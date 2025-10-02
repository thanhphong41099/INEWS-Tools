using System.Collections.Generic;
using News2025.Services;
using KAsyncEngineLib;

namespace News2025.Logic
{
    public class ScenePlayerService
    {
        private readonly IKarismaCG3Model _karismaCG3;
        private readonly IAccessService _accessService;
        private readonly Dictionary<string, int> _layers;
        private readonly string _workingFolder;

        public ScenePlayerService(IKarismaCG3Model karismaCG3, IAccessService accessService, Dictionary<string, int> layers, string workingFolder)
        {
            _karismaCG3 = karismaCG3;
            _accessService = accessService;
            _layers = layers;
            _workingFolder = workingFolder;
        }

        public void PlaySimpleScene(string name, string table, string column)
        {
            string scene = _accessService.QueryColumnValue(table, column, "Description", name);
            if (!string.IsNullOrEmpty(scene) && _layers.TryGetValue(table, out int layer))
            {
                _karismaCG3.PlayScene(_workingFolder + scene, layer);
            }
        }

        public void PlaySceneNameDes(string sceneKey, string name, string description)
        {
            string scene = _accessService.QueryColumnValue("General", "Name", "Scene", sceneKey);
            if (!string.IsNullOrEmpty(scene) && _layers.TryGetValue(sceneKey, out int layer))
            {
                _karismaCG3.PlaySceneLT(_workingFolder + scene, layer, name, description);
            }
        }

        public void PlayScenePVB(string sceneKey, string name1, string description1, string name2, string description2)
        {
            string scene = _accessService.QueryColumnValue("General", "Name", "Scene", sceneKey);
            if (!string.IsNullOrEmpty(scene) && _layers.TryGetValue(sceneKey, out int layer))
            {
                _karismaCG3.PlayPV2(_workingFolder + scene, layer, name1, description1, name2, description2);
            }
        }
    }
}
