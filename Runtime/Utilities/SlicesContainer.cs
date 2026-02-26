using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rehawk.UIFramework
{
    [Serializable]
    public class SlicesContainer
    { 
#if !ODIN_INSPECTOR  
        [SubclassSelector]
#endif
        [SerializeReference] private UIVirtualContextControlBase[] _slices;

        private static readonly SliceModelBase[] _emptyModels = Array.Empty<SliceModelBase>();
        private SliceModelBase[] _modelsArray = _emptyModels;
        
        public IEnumerable<SliceModelBase> Models
        {
            get => _modelsArray;
            set
            {
                _modelsArray = value switch
                {
                    null => _emptyModels,
                    SliceModelBase[] directArray => directArray,
                    ICollection<SliceModelBase> collection => CopyCollection(collection),
                    _ => CopyEnumerable(value)
                };

                if (_slices == null)
                {
                    return;
                }

                for (int i = 0; i < _slices.Length; i++)
                {
                    UIVirtualContextControlBase slice = _slices[i];

                    if (slice is not IUIContextControl<SliceModelBase> sliceContextControl)
                    {
                        Debug.LogWarning($"Slice does not implement IUIContextControl<SliceModelBase>. [sliceType={slice.GetType().Name}]");
                        slice.SetContext<object>(null);
                        continue;
                    }

                    Type expectedModelType = sliceContextControl.ContextBaseType;
                    SliceModelBase matchingModel = null;

                    for (int m = 0; m < _modelsArray.Length; m++)
                    {
                        SliceModelBase candidate = _modelsArray[m];
                        if (candidate != null && expectedModelType.IsInstanceOfType(candidate))
                        {
                            matchingModel = candidate;
                            break;
                        }
                    }

                    slice.SetContext(matchingModel);
                }
            }
        }

        public void Init()
        {
            for (int i = 0; i < _slices.Length; i++)
            {
                _slices[i].Init();
            }
        }

        public void Release()
        {
            for (int i = 0; i < _slices.Length; i++)
            {
                _slices[i].Release();
            }
        }
        
        private static SliceModelBase[] CopyCollection(ICollection<SliceModelBase> collection)
        {
            var result = new SliceModelBase[collection.Count];
            collection.CopyTo(result, 0);
            return result;
        }

        private static SliceModelBase[] CopyEnumerable(IEnumerable<SliceModelBase> enumerable)
        {
            // Fallback for non-collection enumerables.
            return new List<SliceModelBase>(enumerable).ToArray();
        }
    }
}
