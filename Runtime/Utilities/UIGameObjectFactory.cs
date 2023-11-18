using UnityEngine;

namespace Rehawk.UIFramework
{
    public delegate GameObject CreateDelegate(GameObject prefab, Transform parent);
    public delegate void DestroyDelegate(GameObject gameObject);
    
    public static class UIGameObjectFactory
    {
        private static CreateDelegate createAction;
        private static DestroyDelegate destroyAction;

        public static void Setup(CreateDelegate createAction, DestroyDelegate destroyAction)
        {
            UIGameObjectFactory.createAction = createAction;
            UIGameObjectFactory.destroyAction = destroyAction;
        }

        internal static GameObject Create(GameObject prefab, Transform parent)
        {
            return createAction.Invoke(prefab, parent);
        }
        
        internal static void Destroy(GameObject gameObject)
        {
            destroyAction.Invoke(gameObject);
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            Setup(Object.Instantiate, Object.Destroy);
        }
    }
}