using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News2025.Services;

namespace News2025.Services
{
    public class DualKarismaCG3Model : IKarismaCG3Model
    {
        private readonly IKarismaCG3Model _main = new KarismaCG3Model();
        private readonly IKarismaCG3Model _backup = new KarismaCG3Model();

        private bool _useMain = true;
        private bool _useBackup = false;

        public void EnableMain(bool enable) => _useMain = enable;
        public void EnableBackup(bool enable) => _useBackup = enable;

        public void ConnectCG(string ipAddress, string port)
        {
            throw new NotSupportedException("Use ConnectMain/Backup individually.");
        }

        public void ConnectMainCG(string ip, string port) => _main.ConnectCG(ip, port);
        public void ConnectBackupCG(string ip, string port) => _backup.ConnectCG(ip, port);

        public void DisconnectCG()
        {
            if (_useMain) _main.DisconnectCG();
            if (_useBackup) _backup.DisconnectCG();
        }

        public void PlayScene(string scenePath, int layer)
        {
            if (_useMain) _main.PlayScene(scenePath, layer);
            if (_useBackup) _backup.PlayScene(scenePath, layer);
        }
        public void PlaySceneBar(string scenePath, int layer, string barBath)
        {
            if (_useMain) _main.PlaySceneBar(scenePath, layer, barBath);
            if (_useBackup) _backup.PlaySceneBar(scenePath, layer, barBath);
        }
        public void PlaySceneBarValue1(string scenePath, int layer, string Object, string Value, string barPath)
        {
            if (_useMain) _main.PlaySceneBarValue1(scenePath, layer, Object, Value, barPath);
            if (_useBackup) _backup.PlaySceneBarValue1(scenePath, layer, Object, Value, barPath);
        }

        public void PlaySceneScrollStartBar(string scenePath, int layer, int speed, List<string> text, string barPath)
        {
            if (_useMain) _main.PlaySceneScrollStartBar(scenePath, layer, speed, text, barPath);
            if (_useBackup) _backup.PlaySceneScrollStartBar(scenePath, layer, speed, text, barPath);
        }

        public bool PlayTextVNtrans(string scenePath, int layer, string oldText, string newText, bool isOldTilte, bool isNewTitle)
        {
            if (_useMain) return _main.PlayTextVNtrans(scenePath, layer, oldText, newText, isOldTilte, isNewTitle);
            if (_useBackup) return _backup.PlayTextVNtrans(scenePath, layer, oldText, newText, isOldTilte, isNewTitle);
            return false;
        }

        public bool PlayTextVN(string scenePath, int layer, string title, string content)
        {
            if (_useMain) return _main.PlayTextVN(scenePath, layer, title, content);
            if (_useBackup) return _backup.PlayTextVN(scenePath, layer, title, content);
            return false;
        }
        public void TriggerSceneLT(int layer, string line1, string line2)
        {
            if (_useMain) _main.TriggerSceneLT(layer, line1, line2);
            if (_useBackup) _backup.TriggerSceneLT(layer, line1, line2);
        }

        public bool PlaySceneLTPosition(string scenePath, int layer, string line1, string line2, int X, int Y)
        {
            if (_useMain) return _main.PlaySceneLTPosition(scenePath, layer, line1, line2, X, Y);
            if (_useBackup) return _backup.PlaySceneLTPosition(scenePath, layer, line1, line2, X, Y);
            return false;
        }
        public void TriggerSceneLT2(int layer, string line1, string line2)
        {
            if (_useMain) _main.TriggerSceneLT2(layer, line1, line2);
            if (_useBackup) _backup.TriggerSceneLT2(layer, line1, line2);
        }

        public void PlaySceneUTC(string scenePath, int layer, string clock)
        {
            if (_useMain) _main.PlaySceneUTC(scenePath, layer, clock);
            if (_useBackup) _backup.PlaySceneUTC(scenePath, layer, clock);
        }

        public void PlayTextTransition(string scenePath, int layer, string line1out, string line2out, string line1in, string line2in)
        {
            if (_useMain) _main.PlayTextTransition(scenePath, layer, line1out, line2out, line1in, line2in);
            if (_useBackup) _backup.PlayTextTransition(scenePath, layer, line1out, line2out, line1in, line2in);
        }
        public void PlayTextTransitionPosition(string scenePath, int layer, string line1out, string line2out, string line1in, string line2in, int X, int Y)
        {
            if (_useMain) _main.PlayTextTransitionPosition(scenePath, layer, line1out, line2out, line1in, line2in, X, Y);
            if (_useBackup) _backup.PlayTextTransitionPosition(scenePath, layer, line1out, line2out, line1in, line2in, X, Y);
        }

        public void PlaySceneName(string scenePath, int layer, string name)
        {
            if (_useMain) _main.PlaySceneName(scenePath, layer, name);
            if (_useBackup) _backup.PlaySceneName(scenePath, layer, name);
        }

        public void PlayScene2(string scenePath, int layer, string line1, string line2)
        {
            if (_useMain) _main.PlayScene2(scenePath, layer, line1, line2);
            if (_useBackup) _backup.PlayScene2(scenePath, layer, line1, line2);
        }

        public bool PlaySceneLT(string scenePath, int layer, string name, string description)
        {
            if (_useMain) return _main.PlaySceneLT(scenePath, layer, name, description);
            if (_useBackup) return _backup.PlaySceneLT(scenePath, layer, name, description);
            return false;
        }

        public void PlayScene3Line(string scenePath, int layer, string name, string description, string description2)
        {
            if (_useMain) _main.PlayScene3Line(scenePath, layer, name, description, description2);
            if (_useBackup) _backup.PlayScene3Line(scenePath, layer, name, description, description2);
        }

        public void PlayPV2(string scenePath, int layer, string name1, string description1, string name2, string description2)
        {
            if (_useMain) _main.PlayPV2(scenePath, layer, name1, description1, name2, description2);
            if (_useBackup) _backup.PlayPV2(scenePath, layer, name1, description1, name2, description2);
        }

        public void PlayScenePVL(string scenePath, int layer, string name1, string description1, string description2)
        {
            if (_useMain) _main.PlayScenePVL(scenePath, layer, name1, description1, description2);
            if (_useBackup) _backup.PlayScenePVL(scenePath, layer, name1, description1, description2);
        }

        public void PlayPV3(string scenePath, int layer, string name1, string description1, string name2, string description2, string name3, string description3)
        {
            if (_useMain) _main.PlayPV3(scenePath, layer, name1, description1, name2, description2, name3, description3);
            if (_useBackup) _backup.PlayPV3(scenePath, layer, name1, description1, name2, description2, name3, description3);
        }
        public void PlayPV4(string scenePath, int layer, string name1, string description1, string name2, string description2, string name3, string description3, string name4, string description4)
        {
            if (_useMain) _main.PlayPV4(scenePath, layer, name1, description1, name2, description2, name3, description3, name4, description4);
            if (_useBackup) _backup.PlayPV4(scenePath, layer, name1, description1, name2, description2, name3, description3, name4, description4);
        }
        public void PlaySceneTroiNgang(string scenePath, int layer, int speed, string text)
        {
            if (_useMain) _main.PlaySceneTroiNgang(scenePath, layer, speed, text);
            if (_useBackup) _backup.PlaySceneTroiNgang(scenePath, layer, speed, text);
        }

        public void PlaySceneScrollStart(string scenePath, int layer, int speed, List<string> text)
        {
            if (_useMain) _main.PlaySceneScrollStart(scenePath, layer, speed, text);
            if (_useBackup) _backup.PlaySceneScrollStart(scenePath, layer, speed, text);
        }

        public void Stop(int layer)
        {
            if (_useMain) _main.Stop(layer);
            if (_useBackup) _backup.Stop(layer);
        }

        public void PlayOut(int layer)
        {
            if (_useMain) _main.PlayOut(layer);
            if (_useBackup) _backup.PlayOut(layer);
        }

        public void StopAll()
        {
            if (_useMain) _main.StopAll();
            if (_useBackup) _backup.StopAll();
        }
        public void UnloadAll()
        {
            if (_useMain) _main.UnloadAll();
            if (_useBackup) _backup.UnloadAll();
        }

        public void ClearLog()
        {
            _main.ClearLog();
            _backup.ClearLog();
        }

        public void timerTick(int layer)
        {
            if (_useMain) _main.timerTick(layer);
            if (_useBackup) _backup.timerTick(layer);
        }

        public void updateTxtCrawl(List<string> text)
        {
            _main.updateTxtCrawl(text);
            _backup.updateTxtCrawl(text);
        }

        public int space
        {
            get => _main.space;
            set
            {
                _main.space = value;
                _backup.space = value;
            }
        }
        public string seperator
        {
            get => _main.seperator;
            set
            {
                _main.seperator = value;
                _backup.seperator = value;
            }
        }

        public string LogText => _main.LogText + "\n" + _backup.LogText;
        public void ExportThumbnailImage(string scenePath, string outputPath, int width, int height, int frame = -1)
        {
            if (_useMain) _main.ExportThumbnailImage(scenePath, outputPath, width, height, frame);
        }

        // 1) OneLine In/Out
        public void PlayOneLineIn(string scenePath, int layer, string text)
        {
            if (_useMain) _main.PlayOneLineIn(scenePath, layer, text);
            if (_useBackup) _backup.PlayOneLineIn(scenePath, layer, text);
        }

        public void PlayOneLineOut(string scenePath, int layer, string text)
        {
            if (_useMain) _main.PlayOneLineOut(scenePath, layer, text);
            if (_useBackup) _backup.PlayOneLineOut(scenePath, layer, text);
        }

        // 2) TwoLine In/Out
        public void PlayTwoLineIn(string scenePath, int layer, string line1, string line2)
        {
            if (_useMain) _main.PlayTwoLineIn(scenePath, layer, line1, line2);
            if (_useBackup) _backup.PlayTwoLineIn(scenePath, layer, line1, line2);
        }

        public void PlayTwoLineOut(string scenePath, int layer, string line1, string line2)
        {
            if (_useMain) _main.PlayTwoLineOut(scenePath, layer, line1, line2);
            if (_useBackup) _backup.PlayTwoLineOut(scenePath, layer, line1, line2);
        }

        // 3) Content (3+ line) In/Out
        public void PlayContentIn(string scenePath, int layer, string text)
        {
            if (_useMain) _main.PlayContentIn(scenePath, layer, text);
            if (_useBackup) _backup.PlayContentIn(scenePath, layer, text);
        }

        public void PlayContentOut(string scenePath, int layer, string text)
        {
            if (_useMain) _main.PlayContentOut(scenePath, layer, text);
            if (_useBackup) _backup.PlayContentOut(scenePath, layer, text);
        }

        // 4) Transitions
        public void PlayTextTitleTrans(string scenePath, int layer, string oldText, string newText)
        {
            if (_useMain) _main.PlayTextTitleTrans(scenePath, layer, oldText, newText);
            if (_useBackup) _backup.PlayTextTitleTrans(scenePath, layer, oldText, newText);
        }

        public void PlayTextTitleTo2Line(string scenePath, int layer, string oldText, string newLine1, string newLine2)
        {
            if (_useMain) _main.PlayTextTitleTo2Line(scenePath, layer, oldText, newLine1, newLine2);
            if (_useBackup) _backup.PlayTextTitleTo2Line(scenePath, layer, oldText, newLine1, newLine2);
        }

        public void PlayTextTitleTo3Line(string scenePath, int layer, string oldText, string newText)
        {
            if (_useMain) _main.PlayTextTitleTo3Line(scenePath, layer, oldText, newText);
            if (_useBackup) _backup.PlayTextTitleTo3Line(scenePath, layer, oldText, newText);
        }

        public void PlayText2LineToTitle(string scenePath, int layer, string oldLine1, string oldLine2, string newText)
        {
            if (_useMain) _main.PlayText2LineToTitle(scenePath, layer, oldLine1, oldLine2, newText);
            if (_useBackup) _backup.PlayText2LineToTitle(scenePath, layer, oldLine1, oldLine2, newText);
        }

        public void PlayText2LineTrans(string scenePath, int layer, string oldLine1, string oldLine2, string newLine1, string newLine2)
        {
            if (_useMain) _main.PlayText2LineTrans(scenePath, layer, oldLine1, oldLine2, newLine1, newLine2);
            if (_useBackup) _backup.PlayText2LineTrans(scenePath, layer, oldLine1, oldLine2, newLine1, newLine2);
        }

        public void PlayText2LineTo3Line(string scenePath, int layer, string oldLine1, string oldLine2, string newText)
        {
            if (_useMain) _main.PlayText2LineTo3Line(scenePath, layer, oldLine1, oldLine2, newText);
            if (_useBackup) _backup.PlayText2LineTo3Line(scenePath, layer, oldLine1, oldLine2, newText);
        }

        public void PlayText3LineToTitle(string scenePath, int layer, string oldText, string newText)
        {
            if (_useMain) _main.PlayText3LineToTitle(scenePath, layer, oldText, newText);
            if (_useBackup) _backup.PlayText3LineToTitle(scenePath, layer, oldText, newText);
        }

        public void PlayText3LineTo2Line(string scenePath, int layer, string oldText, string newLine1, string newLine2)
        {
            if (_useMain) _main.PlayText3LineTo2Line(scenePath, layer, oldText, newLine1, newLine2);
            if (_useBackup) _backup.PlayText3LineTo2Line(scenePath, layer, oldText, newLine1, newLine2);
        }

        public void PlayText3LineTrans(string scenePath, int layer, string oldText, string newText)
        {
            if (_useMain) _main.PlayText3LineTrans(scenePath, layer, oldText, newText);
            if (_useBackup) _backup.PlayText3LineTrans(scenePath, layer, oldText, newText);
        }

    }
}
