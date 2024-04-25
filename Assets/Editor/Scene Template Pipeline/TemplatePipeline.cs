using UnityEditor.SceneTemplate;
using UnityEngine.SceneManagement;

namespace Assets.Editor
{
    public class TemplatePipeline : ISceneTemplatePipeline
    {
        public virtual void AfterTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, Scene scene, bool isAdditive, string sceneName)
        {
        }

        public virtual void BeforeTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, bool isAdditive, string sceneName)
        {
        }

        public virtual bool IsValidTemplateForInstantiation(SceneTemplateAsset sceneTemplateAsset)
        {
            return true;
        }
    }
}