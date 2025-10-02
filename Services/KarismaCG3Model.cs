using KAsyncEngineLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace News2025.Services
{
    public interface IKarismaCG3Model
    {
        int space { get; set; }
        string LogText { get; }
        string seperator { get; set; }
        void ConnectCG(string ipAddress, string port);
        void DisconnectCG();
        void PlayScene(string scenePath, int layer);
        void PlaySceneBar(string scenePath, int layer, string barBath);
        void TriggerSceneLT(int layer, string line1, string line2);
        void TriggerSceneLT2(int layer, string line1, string line2);
        void PlaySceneUTC(string scenePath, int layer, string clock);
        void PlayTextTransition(string scenePath, int layer, string line1out, string line2out, string line1in, string line2in);
        void PlayTextTransitionPosition(string scenePath, int layer, string line1out, string line2out, string line1in, string line2in, int X, int Y);
        void PlaySceneName(string scenePath, int layer, string name);
        void PlaySceneBarValue1(string scenePath, int layer, string Object, string Value, string barPath);
        void PlayScene2(string scenePath, int layer, string line1, string line2);
        bool PlayTextVN(string scenePath, int layer, string title, string content);
        bool PlayTextVNtrans(string scenePath, int layer, string oldText, string newText, bool isOldTilte, bool isNewTitle);
        bool PlaySceneLT(string scenePath, int layer, string name, string description);
        bool PlaySceneLTPosition(string scenePath, int layer, string line1, string line2, int X, int Y);
        void PlayScene3Line(string scenePath, int layer, string name, string description, string description2);
        void PlayPV2(string scenePath, int layer, string name1, string description1, string name2, string description2);
        void PlayScenePVL(string scenePath, int layer, string name1, string description1, string description2);
        void PlayPV3(string scenePath, int layer, string name1, string description1, string name2, string description2, string name3, string description3);
        void PlayPV4(string scenePath, int layer, string name1, string description1, string name2, string description2, string name3, string description3, string name4, string description4);
        void PlaySceneTroiNgang(string scenePath, int layer, int speed, string text);
        void Stop(int layer);
        void PlayOut(int layer);
        void StopAll();
        void ClearLog();
        void PlaySceneScrollStart(string scenePath, int layer, int speed, List<string> text);
        void PlaySceneScrollStartBar(string scenePath, int layer, int speed, List<string> text, string barPath);
        void timerTick(int layer);
        void updateTxtCrawl(List<string> text);
        void UnloadAll();
        void ExportThumbnailImage(string scenePath, string outputPath, int width, int height, int frame = -1);

        // In/Out — 1 dòng (biến: title)
        void PlayOneLineIn(string scenePath, int layer, string text);
        void PlayOneLineOut(string scenePath, int layer, string text);

        // In/Out — 2 dòng (biến: content1, content2)
        void PlayTwoLineIn(string scenePath, int layer, string line1, string line2);
        void PlayTwoLineOut(string scenePath, int layer, string line1, string line2);

        // In/Out — 3+ dòng (biến: content)
        void PlayContentIn(string scenePath, int layer, string text);
        void PlayContentOut(string scenePath, int layer, string text);

        // Transitions
        // Title -> Title (biến: titleOut, titleIn)
        void PlayTextTitleTrans(string scenePath, int layer, string oldText, string newText);

        // Title -> 2Line (biến: titleOut, content1In, content2In)
        void PlayTextTitleTo2Line(string scenePath, int layer, string oldText, string newLine1, string newLine2);

        // Title -> 3Line (biến: titleOut, contentIn)
        void PlayTextTitleTo3Line(string scenePath, int layer, string oldText, string newText);

        // 2Line -> Title (biến: content1Out, content2Out, titleIn)
        void PlayText2LineToTitle(string scenePath, int layer, string oldLine1, string oldLine2, string newText);

        // 2Line -> 2Line (biến: content1Out, content2Out, content1In, content2In)
        void PlayText2LineTrans(string scenePath, int layer, string oldLine1, string oldLine2, string newLine1, string newLine2);

        // 2Line -> 3Line (biến: content1Out, content2Out, contentIn)
        void PlayText2LineTo3Line(string scenePath, int layer, string oldLine1, string oldLine2, string newText);

        // 3Line -> Title (biến: contentOut, titleIn)
        void PlayText3LineToTitle(string scenePath, int layer, string oldText, string newText);

        // 3Line -> 2Line (biến: contentOut, content1In, content2In)
        void PlayText3LineTo2Line(string scenePath, int layer, string oldText, string newLine1, string newLine2);

        // 3Line -> 3Line (biến: contentOut, contentIn)
        void PlayText3LineTrans(string scenePath, int layer, string oldText, string newText);
    }

    public class KarismaCG3Model : IKarismaCG3Model
    {

        protected KAEngine KAEngine;
        protected KAScenePlayer KAScenePlayer;
        protected KAScene KAScene;
        protected KAScene KASceneCrawl;
        protected KAExporter exporter;
        protected CG3EventHandler EventHandler;

        public KAObject KAObject;

        public int SelectedIndex { get; set; } = 0;
        public string SelectedObject { get; set; } = "";

        private StringBuilder _logBuilder = new StringBuilder();
        public string LogText => _logBuilder.ToString();

        public int _layer { get; set; } = 0;
        public string seperator { get; set;  } = "     ";
        public int space { get; set; } = 10; // khoảng cách giữa các dòng trong scroll text

        public KarismaCG3Model()
        {
            try
            {
                KAEngine = new KAEngine();
                EventHandler = new CG3EventHandler(this);
                OnLogMessage("KarismaCG3Model initialized.");
            }
            catch (Exception e)
            {
                OnLogMessage("Error initializing: " + e.Message);
            }
        }

        public void ConnectCG(string ipAddress, string port)
        {
            if (KAEngine == null)
            {
                OnLogMessage("Error: KAEngine is not initialized.");
                return;
            }

            try
            {
                KAEngine.Connect(ipAddress, Convert.ToInt32(port), EventHandler);
                KAScenePlayer = KAEngine.GetScenePlayer(0);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error connecting to CG: {ex.Message}");
            }
        }

        public void DisconnectCG()
        {
            KAEngine?.Disconnect();
            OnLogMessage("Disconnected KarismaCG3.");
        }
        public void PlayScene(string scenePath, int layer)
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return;
            }
            try
            {
                KAScene = KAEngine.LoadScene(scenePath, scenePath);
                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }
        public void PlaySceneBar(string scenePath, int layer, string barBath)
        {
            try
            {
                KAScene = KAEngine.LoadScene(scenePath, scenePath);

                KAEngine.BeginTransaction();

                KAObject = KAScene.GetObject("bar");
                KAObject.SetValue(barBath);

                KAEngine.EndTransaction();

                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }
        public void PlayScene2(string scenePath, int layer, string line1, string line2)
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return;
            }
            try
            {
                KAScene = KAEngine.LoadScene(scenePath, scenePath);

                KAEngine.BeginTransaction();
                SetObjectValue("line1", line1);
                SetObjectValue("line2", line2);

                KAEngine.EndTransaction();
                Thread.Sleep(50);

                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }
        public void PlaySceneUTC(string scenePath, int layer, string time)
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return;
            }
            try
            {
                time = string.IsNullOrEmpty(time) ? string.Empty : time;

                KAScene = KAEngine.LoadScene(scenePath, scenePath);

                KAEngine.BeginTransaction();
                SetObjectValue("clock", time);

                KAEngine.EndTransaction();
                Thread.Sleep(50);

                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }
        public void PlaySceneName(string scenePath, int layer, string name)
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return;
            }
            try
            {
                name = string.IsNullOrEmpty(name) ? string.Empty : name;

                KAScene = KAEngine.LoadScene(scenePath, scenePath);

                KAEngine.BeginTransaction();
                SetObjectValue("name", name);

                KAEngine.EndTransaction();
                Thread.Sleep(50);

                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }

        public void PlaySceneBarValue1(string scenePath, int layer, string Object, string Value, string barPath)
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return;
            }
            try
            {
                KAScene = KAEngine.LoadScene(scenePath, scenePath);
                KAEngine.BeginTransaction();
                SetObjectValue(Object, Value);
                SetObjectValue("bar", barPath);
                KAEngine.EndTransaction();
                Thread.Sleep(50);
                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }
        public void PlayTextTransition(string scenePath, int layer, string line1out, string line2out, string line1in, string line2in)
        {
            try
            {
                // Normalize input strings to avoid null reference exceptions
                line1out = line1out ?? string.Empty;
                line2out = line2out ?? string.Empty;
                line1in = line1in ?? string.Empty;
                line2in = line2in ?? string.Empty;

                KAScene = KAEngine.LoadScene(scenePath, scenePath);
                KAEngine.BeginTransaction();

                // Process outgoing text
                bool useRegularOutFormat = !string.IsNullOrEmpty(line2out) || !(line1out.IndexOf("Thực hiện", StringComparison.OrdinalIgnoreCase) < 0);
                SetObjectValue("line1-out", useRegularOutFormat ? line1out : "");
                SetObjectValue("line2-out", useRegularOutFormat ? line2out : "");
                SetObjectValue("title-out", useRegularOutFormat ? "" : line1out);

                // Process incoming text
                bool useRegularInFormat = !string.IsNullOrEmpty(line2in) || !(line1in.IndexOf("Thực hiện", StringComparison.OrdinalIgnoreCase) < 0);
                SetObjectValue("line1-in", useRegularInFormat ? line1in : "");
                SetObjectValue("line2-in", useRegularInFormat ? line2in : "");
                SetObjectValue("title-in", useRegularInFormat ? "" : line1in);

                KAEngine.EndTransaction();

                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }

        public void PlayTextTransitionPosition(string scenePath, int layer, string line1out, string line2out, string line1in, string line2in, int X, int Y)
        {
            try
            {
                // Normalize input strings to avoid null reference exceptions
                line1out = line1out ?? string.Empty;
                line2out = line2out ?? string.Empty;
                line1in = line1in ?? string.Empty;
                line2in = line2in ?? string.Empty;

                KAScene = KAEngine.LoadScene(scenePath, scenePath);
                KAEngine.BeginTransaction();

                SetAttributeText(X, Y);

                // Process outgoing text
                bool useRegularOutFormat = !string.IsNullOrEmpty(line2out) || !(line1out.IndexOf("Thực hiện", StringComparison.OrdinalIgnoreCase) < 0);
                SetObjectValue("line1-out", useRegularOutFormat ? line1out : "");
                SetObjectValue("line2-out", useRegularOutFormat ? line2out : "");
                SetObjectValue("title-out", useRegularOutFormat ? "" : line1out);

                // Process incoming text
                bool useRegularInFormat = !string.IsNullOrEmpty(line2in) || !(line1in.IndexOf("Thực hiện", StringComparison.OrdinalIgnoreCase) < 0);
                SetObjectValue("line1-in", useRegularInFormat ? line1in : "");
                SetObjectValue("line2-in", useRegularInFormat ? line2in : "");
                SetObjectValue("title-in", useRegularInFormat ? "" : line1in);

                KAEngine.EndTransaction();

                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }
        public void SetAttributeText(int X, int Y)
        {
            KAObject = KAScene.GetObject("TextIn");
            KAObject?.SetPosition(X, Y, 0, eKVectorType.VECTOR_TYPE_XY);
            KAObject = KAScene.GetObject("TextOut");
            KAObject?.SetPosition(X, Y, 0, eKVectorType.VECTOR_TYPE_XY);
        }
        public void TriggerSceneLT(int layer, string line1, string line2)
        {
            try
            {
                KAScene KAScene = KAScenePlayer.GetPlayingScene(layer);
                if (KAScene != null)
                {
                    if (!string.IsNullOrEmpty(line2) ||
                        !(line1.IndexOf("Thực hiện", StringComparison.OrdinalIgnoreCase) < 0))
                    {
                        KAObject = KAScene.GetObject("line1-in");
                        KAObject.SetValue(line1);
                        KAObject = KAScene.GetObject("line2-in");
                        KAObject.SetValue(line2);
                        KAObject = KAScene.GetObject("title-in");
                        KAObject.SetValue("");
                    }
                    else
                    {
                        KAObject = KAScene.GetObject("line1-in");
                        KAObject.SetValue("");
                        KAObject = KAScene.GetObject("line2-in");
                        KAObject.SetValue("");
                        KAObject = KAScene.GetObject("title-in");
                        KAObject.SetValue(line1);
                    }
                }
                KAScenePlayer.Trigger(layer, "transG1-G2");

            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }
        public void TriggerSceneLT2(int layer, string line1, string line2)
        {
            try
            {
                KAScene KAScene = KAScenePlayer.GetPlayingScene(layer);
                if (KAScene != null)
                {
                    if (string.IsNullOrEmpty(line1))
                    {
                        return;
                    }
                    if (!string.IsNullOrEmpty(line2) ||
                        !(line1.IndexOf("Thực hiện", StringComparison.OrdinalIgnoreCase) < 0))
                    {
                        KAObject = KAScene.GetObject("line1");
                        KAObject.SetValue(line1);
                        KAObject = KAScene.GetObject("line2");
                        KAObject.SetValue(line2);
                        KAObject = KAScene.GetObject("title");
                        KAObject.SetValue("");
                    }
                    else
                    {
                        KAObject = KAScene.GetObject("line1");
                        KAObject.SetValue("");
                        KAObject = KAScene.GetObject("line2");
                        KAObject.SetValue("");
                        KAObject = KAScene.GetObject("title");
                        KAObject.SetValue(line1);
                    }
                }

                KAScenePlayer.Trigger(layer, "transG2-G1");
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }
        public bool PlayTextVN(string scenePath, int layer, string title, string content)
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return false;
            }

            try
            {
                if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(content))
                {
                    // Không có dữ liệu gì để hiển thị
                    return false;
                }

                // Load scene
                KAScene = KAEngine.LoadScene(scenePath, scenePath);
                KAEngine.BeginTransaction();

                // Gán Title
                if (!string.IsNullOrEmpty(title))
                    SetObjectValue("title", title);
                else
                    SetObjectValue("title", string.Empty);


                // Gán Content
                if (!string.IsNullOrEmpty(content))
                    SetObjectValue("content", content);
                else
                {
                    SetObjectValue("content", string.Empty);
                }

                KAEngine.EndTransaction();
                Thread.Sleep(50);

                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
                return true;
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
                return false;
            }
        }

        public bool PlayTextVNtrans(string scenePath, int layer, string oldText, string newText, bool isOldTilte, bool isNewTitle)
        {
            try
            {
                if (string.IsNullOrEmpty(oldText) && string.IsNullOrEmpty(newText))
                {return false;}

                KAScene = KAEngine.LoadScene(scenePath, scenePath);
                KAEngine.BeginTransaction();
                if (isOldTilte)
                {
                    SetObjectValue("titleOut", oldText);
                    SetObjectValue("contentOut", string.Empty);
                }
                else
                {
                    SetObjectValue("titleOut", string.Empty);
                    SetObjectValue("contentOut", oldText);
                }

                if (isNewTitle)
                {
                    SetObjectValue("titleIn", newText);
                    SetObjectValue("contentIn", string.Empty);
                }
                else
                {
                    SetObjectValue("titleIn", string.Empty);
                    SetObjectValue("contentIn", newText);
                }
                KAEngine.EndTransaction();
                Thread.Sleep(50);
                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
                return true;
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
                return false;
            }
        }




        public bool PlaySceneLT(string scenePath, int layer, string line1, string line2)
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return false;
            }
            try
            {
                line1 = string.IsNullOrEmpty(line1) ? string.Empty : line1;
                line2 = string.IsNullOrEmpty(line2) ? string.Empty : line2;

                if (string.IsNullOrEmpty(line1))
                {
                    return false;
                }

                KAScene = KAEngine.LoadScene(scenePath, scenePath);
                KAEngine.BeginTransaction();

                if (!string.IsNullOrEmpty(line2) ||
                    !(line1.IndexOf("Thực hiện", StringComparison.OrdinalIgnoreCase) < 0))
                {
                    SetObjectValue("line1", line1);
                    SetObjectValue("line2", line2);
                    SetObjectValue("title", string.Empty);
                }
                else
                {
                    SetObjectValue("title", line1);
                    SetObjectValue("line1", string.Empty);
                    SetObjectValue("line2", string.Empty);
                }

                KAEngine.EndTransaction();
                Thread.Sleep(50);

                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
                return true;
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
                return false;
            }
        }

        public bool PlaySceneLTPosition(string scenePath, int layer, string line1, string line2, int X, int Y)
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return false;
            }
            try
            {
                line1 = string.IsNullOrEmpty(line1) ? string.Empty : line1;
                line2 = string.IsNullOrEmpty(line2) ? string.Empty : line2;

                if (string.IsNullOrEmpty(line1))
                {
                    return false;
                }

                KAScene = KAEngine.LoadScene(scenePath, scenePath);
                KAEngine.BeginTransaction();

                SetAttributeText(X, Y);

                if (!string.IsNullOrEmpty(line2) ||
                    !(line1.IndexOf("Thực hiện", StringComparison.OrdinalIgnoreCase) < 0))
                {
                    SetObjectValue("line1", line1);
                    SetObjectValue("line2", line2);
                    SetObjectValue("title", string.Empty);
                }
                else
                {
                    SetObjectValue("title", line1);
                    SetObjectValue("line1", string.Empty);
                    SetObjectValue("line2", string.Empty);
                }

                KAEngine.EndTransaction();
                Thread.Sleep(50);

                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
                return true;
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
                return false;
            }
        }
        public void PlayScene3Line(string scenePath, int layer, string name, string description, string description2)
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return;
            }
            try
            {
                name = string.IsNullOrEmpty(name) ? string.Empty : name;
                description = string.IsNullOrEmpty(description) ? string.Empty : description;

                KAScene = KAEngine.LoadScene(scenePath, scenePath);
                KAEngine.BeginTransaction();

                SetObjectValue("name", name);
                SetObjectValue("description", description);
                SetObjectValue("description2", description2 );

                KAEngine.EndTransaction();
                Thread.Sleep(50);

                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }
        public void PlayPV2(string scenePath, int layer, string name1, string description1, string name2, string description2)
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return;
            }
            try
            {
                name1 = string.IsNullOrEmpty(name1) ? string.Empty : name1;
                description1 = string.IsNullOrEmpty(description1) ? string.Empty : description1;
                name2 = string.IsNullOrEmpty(name2) ? string.Empty : name2;
                description2 = string.IsNullOrEmpty(description2) ? string.Empty : description2;

                KAScene = KAEngine.LoadScene(scenePath, scenePath);
                KAEngine.BeginTransaction();

                SetObjectValue("name1", name1);
                SetObjectValue("des1", description1);
                SetObjectValue("name2", name2);
                SetObjectValue("des2", description2);

                KAEngine.EndTransaction();
                Thread.Sleep(50);

                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }
        public void PlaySceneTroiNgang(string scenePath, int layer, int speed, string text)
        {
            try
            {
                text = string.IsNullOrEmpty(text) ? string.Empty : text;

                KAScene = KAEngine.LoadScene(scenePath, scenePath);

                KAEngine.BeginTransaction();
                SetObjectValue("text", text);
                IKAScroll scrollGroup = (IKAScroll)KAScene.GetObject("Crawl");
                scrollGroup.SetScrollSpeed(speed);
                KAEngine.EndTransaction();

                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }
        public void PlaySceneScrollStart(string scenePath, int layer, int speed, List<string> text)
        {
            LoadScrollQueue(text);

            try
            {
                KASceneCrawl = KAEngine.LoadScene(scenePath, scenePath);
                KAScroll scroll = (KAScroll)KASceneCrawl.GetObject("Crawl");
                scroll.InitScrollObject();
                scroll.SetScrollSpeed(speed);

                KAScenePlayer.Prepare(layer, KASceneCrawl);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }

        public void PlaySceneScrollStartBar(string scenePath, int layer, int speed, List<string> text, string barPath)
        {
            LoadScrollQueue(text);

            try
            {
                KASceneCrawl = KAEngine.LoadScene(scenePath, scenePath);
                KAEngine.BeginTransaction();

                KAObject = KASceneCrawl.GetObject("bar");
                KAObject.SetValue(barPath);

                KAScroll scroll = (KAScroll)KASceneCrawl.GetObject("Crawl");
                scroll.InitScrollObject();
                scroll.SetScrollSpeed(speed);
                KAEngine.EndTransaction();

                KAScenePlayer.Prepare(layer, KASceneCrawl);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }
        private Queue<ScrollItem> _scrollQueue = new Queue<ScrollItem>();
        private List<ScrollItem> _initialScrollItems = new List<ScrollItem>();
        public void LoadScrollQueue(IEnumerable<string> items)
        {
            _scrollQueue.Clear();
            _initialScrollItems.Clear();

            var itemList = items.Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)).ToList();
            if (itemList.Count == 0) return;

            for (int i = 0; i < itemList.Count; i++)
            {
                var content = new ScrollItem { Text = itemList[i], IsSeparator = false };
                _scrollQueue.Enqueue(content);
                _initialScrollItems.Add(content);

                if (i < itemList.Count - 1)
                {
                    var separator = new ScrollItem { Text = seperator, IsSeparator = true };
                    _scrollQueue.Enqueue(separator);
                    _initialScrollItems.Add(separator);
                }
            }

            // ⚠️ Chèn thêm 1 separator cuối cùng để nối tin cuối → tin đầu khi vòng lặp xảy ra
            var tailSeparator = new ScrollItem { Text = seperator, IsSeparator = true };
            _scrollQueue.Enqueue(tailSeparator);
            _initialScrollItems.Add(tailSeparator);
        }

        // Check scroll remaining distance for scroll object
        public void timerTick(int layer)
        {
            KAScene KASceneCrawl = KAScenePlayer.GetPlayingScene(layer);
            _layer = layer;
            KAScroll KAScroll = (KAScroll)KASceneCrawl.GetObject("Crawl");
            KAScroll.QueryScrollRemainingDistance();
        }
        public void updateTxtCrawl(List<string> text)
        {
            LoadScrollQueue(text);
        }
        public void OnQueryScrollRemainingDistance(int ScrollRemainingDistance)
        {
            if (ScrollRemainingDistance < 3000)
            {
                if (_scrollQueue.Count == 0 && _initialScrollItems.Count > 0)
                {
                    // Nạp lại để chạy lặp vô hạn
                    foreach (var item in _initialScrollItems)
                        _scrollQueue.Enqueue(new ScrollItem { Text = item.Text, IsSeparator = item.IsSeparator });
                }

                if (_scrollQueue.Count == 0) return;

                var nextItem = _scrollQueue.Dequeue();

                // Load scene và đối tượng hiện tại
                KAObject scrollObject = KASceneCrawl.GetObject(nextItem.IsSeparator ? "SeparatorArticle" : "Article");
                scrollObject.SetValue(nextItem.Text);

                KAScene KAScene = KAScenePlayer.GetPlayingScene(_layer);
                KAScroll KAScroll = (KAScroll)KAScene.GetObject("Crawl");
                KAScroll.AddScrollObject(scrollObject, 0);
            }
        }


        private void SetObjectValue(string objectName, string value)
        {
            KAObject = KAScene.GetObject(objectName);
            KAObject.SetValue(value);
        }

        public void OnLogMessage(string message)
        {
            string log = $"{DateTime.Now:HH:mm:ss} - {message}";
            _logBuilder.AppendLine(log);
            Console.WriteLine(log); // để hiển thị log trong debug window nếu cần
        }

        public void OnLoadScene()
        {
            KAScene?.QueryObjectInfos();
            OnLogMessage("Scene loaded and object info queried.");
        }

        public void Stop(int layer)
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return;
            }

            try
            {
                KAScenePlayer?.Stop(layer);
                OnLogMessage($"Stopped scene on layer {layer}");
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error stopping scene on layer {layer}: {ex.Message}");
            }
        }
        public void PlayOut(int layer)
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return;
            }
            try
            {
                KAScenePlayer?.PlayOut(layer);
                OnLogMessage($"Stopped output on layer {layer}");
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error stopping output on layer {layer}: {ex.Message}");
            }
        }

        public void StopAll()
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return;
            }

            try
            {
                KAScenePlayer?.StopAll();
                OnLogMessage("Stopped all scenes.");
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error stopping all scenes: {ex.Message}");
            }
        }

        private bool IsConnected()
        {
            if (KAEngine == null)
            {
                OnLogMessage("Error: KAEngine is not initialized.");
                return false;
            }

            if (KAScenePlayer == null)
            {
                OnLogMessage("Error: KAScenePlayer is not initialized.");
                return false;
            }

            return true;
        }
        public void ClearLog()
        {
            _logBuilder.Clear();
            OnLogMessage("Log cleared.");
        }
        public void PlayScenePVL(string scenePath, int layer, string name1, string description1, string description2)
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return;
            }
            try
            {
                name1 = string.IsNullOrEmpty(name1) ? string.Empty : name1;
                description1 = string.IsNullOrEmpty(description1) ? string.Empty : description1;
                description2 = string.IsNullOrEmpty(description2) ? string.Empty : description2;

                KAScene = KAEngine.LoadScene(scenePath, scenePath);
                KAEngine.BeginTransaction();

                SetObjectValue("name1", name1);
                SetObjectValue("description1", description1);
                SetObjectValue("description2", description2);

                KAEngine.EndTransaction();
                Thread.Sleep(50);

                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }
        public void PlayPV3(string scenePath, int layer, string name1, string description1, string name2, string description2, string name3, string description3)
        {
            if (!IsConnected())
            {
                OnLogMessage("Error: Not connected to CG server.");
                return;
            }
            try
            {
                name1 = string.IsNullOrEmpty(name1) ? string.Empty : name1;
                description1 = string.IsNullOrEmpty(description1) ? string.Empty : description1;
                name2 = string.IsNullOrEmpty(name2) ? string.Empty : name2;
                description2 = string.IsNullOrEmpty(description2) ? string.Empty : description2;
                name3 = string.IsNullOrEmpty(name3) ? string.Empty : name3;
                description3 = string.IsNullOrEmpty(description3) ? string.Empty : description3;

                KAScene = KAEngine.LoadScene(scenePath, scenePath);
                KAEngine.BeginTransaction();

                SetObjectValue("name1", name1);
                SetObjectValue("des1", description1);
                SetObjectValue("name2", name2);
                SetObjectValue("des2", description2);
                SetObjectValue("name3", name3);
                SetObjectValue("des3", description3);

                KAEngine.EndTransaction();
                Thread.Sleep(50);

                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }

        public void PlayPV4(string scenePath, int layer, string name1, string description1, string name2, string description2, string name3, string description3, string name4, string description4)
                {
                    if (!IsConnected())
                    {
                        OnLogMessage("Error: Not connected to CG server.");
                        return;
                    }
                    try
                    {
                        name1 = string.IsNullOrEmpty(name1) ? string.Empty : name1;
                        description1 = string.IsNullOrEmpty(description1) ? string.Empty : description1;
                        name2 = string.IsNullOrEmpty(name2) ? string.Empty : name2;
                        description2 = string.IsNullOrEmpty(description2) ? string.Empty : description2;
                        name3 = string.IsNullOrEmpty(name3) ? string.Empty : name3;
                        description3 = string.IsNullOrEmpty(description3) ? string.Empty : description3;
                        name4 = string.IsNullOrEmpty(name4) ? string.Empty : name4;
                        description4 = string.IsNullOrEmpty(description4) ? string.Empty : description4;
                        KAScene = KAEngine.LoadScene(scenePath, scenePath);
                        KAEngine.BeginTransaction();
                        SetObjectValue("name1", name1);
                        SetObjectValue("des1", description1);
                        SetObjectValue("name2", name2);
                        SetObjectValue("des2", description2);
                        SetObjectValue("name3", name3);
                        SetObjectValue("des3", description3);
                        SetObjectValue("name4", name4);
                        SetObjectValue("des4", description4);
                        KAEngine.EndTransaction();
                        Thread.Sleep(50);
                        KAScenePlayer.Prepare(layer, KAScene);
                        KAScenePlayer.Play(layer);
                    }
                    catch (Exception ex)
                    {
                        OnLogMessage($"Error: {ex.Message}");
                    }
                }
        public void UnloadAll()
        {
            if (KAScene != null)
            {
                try
                {
                    KAEngine.UnloadAll();
                    KAScene = null;
                    OnLogMessage("Scene unloaded successfully.");
                }
                catch (Exception ex)
                {
                    OnLogMessage($"Error unloading scene: {ex.Message}");
                }
            }
            else
            {
                OnLogMessage("No scene to unload.");
            }
        }
        public void ExportThumbnailImage(string scenePath, string outputPath, int width, int height, int frame = -1)
        {
            exporter = new KAExporter();
            KAScene = KAEngine.LoadScene(scenePath, scenePath);
            if (KAScene == null)
            {
                MessageBox.Show("Không load được scene.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            exporter.ExportSceneImage(
                outputPath,
                KAScene,
                width,
                height,
                frame
            );


        }


        #region CRUD Events for Newsline API
        // ======================
        // Helpers dùng chung
        // ======================
        private const int DefaultDelay = 50;

        // Danh sách key chuẩn cần reset nếu không được gán
        private static readonly string[] _knownKeys = new[]
        {
            "title", "content1", "content2", "content",
            "titleOut", "titleIn",
            "content1Out", "content2Out", "contentOut",
            "content1In",  "content2In",  "contentIn"
        };
        // Set value nhưng nuốt exception nếu object không tồn tại
        private void SafeSetObjectValue(string key, string value)
        {
            try
            {
                SetObjectValue(key, value ?? string.Empty);
            }
            catch (Exception ex)
            {
                // Không throw; log nhẹ để debug nếu cần
                OnLogMessage($"[warn] SetObjectValue('{key}') skipped: {ex.Message}");
            }
        }

        private void PlaySceneWithVars(string scenePath, int layer, params (string key, string value)[] vars)
        {
            try
            {
                KAScene = KAEngine.LoadScene(scenePath, scenePath);

                // Gom cặp key/value (nếu có trùng key, lấy cái cuối)
                var effective = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                if (vars != null)
                {
                    foreach (var (key, value) in vars)
                        if (!string.IsNullOrWhiteSpace(key))
                            effective[key] = value ?? string.Empty;
                }

                KAEngine.BeginTransaction();

                // Gán các key được truyền vào
                foreach (var kv in effective)
                    SafeSetObjectValue(kv.Key, kv.Value);

                // Reset các key còn lại về rỗng
                foreach (var k in _knownKeys)
                {
                    if (!effective.ContainsKey(k))
                        SafeSetObjectValue(k, string.Empty);
                }

                KAEngine.EndTransaction();

                Thread.Sleep(DefaultDelay);

                // Prepare + Play
                KAScenePlayer.Prepare(layer, KAScene);
                KAScenePlayer.Play(layer);
            }
            catch (Exception ex)
            {
                OnLogMessage($"Error: {ex.Message}");
            }
        }

        // ======================
        // CRUD call KarismaCG3
        // ======================

        // 1) OneLine In/Out  (biến: title)
        public void PlayOneLineIn(string scenePath, int layer, string text)
        {
            PlaySceneWithVars(scenePath, layer,
                ("title", text)
            );
        }

        public void PlayOneLineOut(string scenePath, int layer, string text)
        {
            PlaySceneWithVars(scenePath, layer,
                ("title", text)
            );
        }

        // 2) TwoLine In/Out (biến: content1, content2)
        public void PlayTwoLineIn(string scenePath, int layer, string line1, string line2)
        {
            PlaySceneWithVars(scenePath, layer,
                ("content1", line1),
                ("content2", line2)
            );
        }

        public void PlayTwoLineOut(string scenePath, int layer, string line1, string line2)
        {
            PlaySceneWithVars(scenePath, layer,
                ("content1", line1),
                ("content2", line2)
            );
        }

        // 3) Content (3+ line) In/Out (biến: content)
        public void PlayContentIn(string scenePath, int layer, string text)
        {
            PlaySceneWithVars(scenePath, layer,
                ("content", text)
            );
        }

        public void PlayContentOut(string scenePath, int layer, string text)
        {
            PlaySceneWithVars(scenePath, layer,
                ("content", text)
            );
        }

        // 4) Transitions
        // 4.1 Title -> Title (biến: titleOut, titleIn)
        public void PlayTextTitleTrans(string scenePath, int layer, string oldText, string newText)
        {
            PlaySceneWithVars(scenePath, layer,
                ("titleOut", oldText),
                ("titleIn", newText)
            );
        }

        // 4.2 Title -> 2Line (biến: titleOut, content1In, content2In)
        public void PlayTextTitleTo2Line(string scenePath, int layer, string oldText, string newLine1, string newLine2)
        {
            PlaySceneWithVars(scenePath, layer,
                ("titleOut", oldText),
                ("content1In", newLine1),
                ("content2In", newLine2)
            );
        }

        // 4.3 Title -> 3Line (biến: titleOut, contentIn)
        public void PlayTextTitleTo3Line(string scenePath, int layer, string oldText, string newText)
        {
            PlaySceneWithVars(scenePath, layer,
                ("titleOut", oldText),
                ("contentIn", newText)
            );
        }

        // 4.4 2Line -> Title (biến: content1Out, content2Out, titleIn)
        public void PlayText2LineToTitle(string scenePath, int layer, string oldLine1, string oldLine2, string newText)
        {
            PlaySceneWithVars(scenePath, layer,
                ("content1Out", oldLine1),
                ("content2Out", oldLine2),
                ("titleIn", newText)
            );
        }

        // 4.5 2Line -> 2Line (biến: content1Out, content2Out, content1In, content2In)
        public void PlayText2LineTrans(string scenePath, int layer, string oldLine1, string oldLine2, string newLine1, string newLine2)
        {
            PlaySceneWithVars(scenePath, layer,
                ("content1Out", oldLine1),
                ("content2Out", oldLine2),
                ("content1In", newLine1),
                ("content2In", newLine2)
            );
        }

        // 4.6 2Line -> 3Line (biến: content1Out, content2Out, contentIn)
        public void PlayText2LineTo3Line(string scenePath, int layer, string oldLine1, string oldLine2, string newText)
        {
            PlaySceneWithVars(scenePath, layer,
                ("content1Out", oldLine1),
                ("content2Out", oldLine2),
                ("contentIn", newText)
            );
        }

        // 4.7 3Line -> Title (biến: contentOut, titleIn)
        public void PlayText3LineToTitle(string scenePath, int layer, string oldText, string newText)
        {
            PlaySceneWithVars(scenePath, layer,
                ("contentOut", oldText),
                ("titleIn", newText)
            );
        }

        // 4.8 3Line -> 2Line (biến: contentOut, content1In, content2In)
        public void PlayText3LineTo2Line(string scenePath, int layer, string oldText, string newLine1, string newLine2)
        {
            PlaySceneWithVars(scenePath, layer,
                ("contentOut", oldText),
                ("content1In", newLine1),
                ("content2In", newLine2)
            );
        }

        // 4.9 3Line -> 3Line (biến: contentOut, contentIn)
        public void PlayText3LineTrans(string scenePath, int layer, string oldText, string newText)
        {
            PlaySceneWithVars(scenePath, layer,
                ("contentOut", oldText),
                ("contentIn", newText)
            );
        }

        #endregion



        // Event handler class
        public class CG3EventHandler : EventHandler
        {
            private readonly KarismaCG3Model _owner;

            public CG3EventHandler(KarismaCG3Model owner)
            {
                _owner = owner;
            }

            public override void OnLoadScene(eKResult result, string sceneName)
            {
                if (result != eKResult.RESULT_SUCCESS)
                {
                    _owner.OnLogMessage($"Error loading scene '{sceneName}': {result}");
                    return;
                }

                _owner.OnLoadScene();
            }

            public override void OnLogMessage(string logMessage)
            {
                _owner.OnLogMessage(logMessage);
            }

            public override void OnQueryObjectInfos(eKResult result, string sceneName, KAObjectInfos objectInfos)
            {
                if (result != eKResult.RESULT_SUCCESS)
                {
                    _owner.OnLogMessage($"Error querying object infos for scene '{sceneName}': {result}");
                    return;
                }

                // Optional: implement OnQueryObjectInfos if needed
                //_owner.OnQueryObjectInfos(objectInfos);
            }
            public override void OnQueryScrollRemainingDistance(eKResult Result, string SceneName, string ObjectName, int ScrollRemainingDistance)
            {
                if (Result != eKResult.RESULT_SUCCESS)
                {
                    _owner.OnLogMessage($"Error querying scroll remaining distance for scene '{SceneName}': {Result}");
                    return;
                }
                // Call the method to handle the scroll remaining distance
                _owner.OnLogMessage($" RESULT_SUCCESS Scroll RD1: {ScrollRemainingDistance}");
                _owner.OnQueryScrollRemainingDistance(ScrollRemainingDistance);
            }

        }
    }
    public class ScrollItem
    {
        public string Text { get; set; }
        public bool IsSeparator { get; set; } = false;
    }
}
