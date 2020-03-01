using Deadbit.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Deadbit.Tools
{
    class TextureController : SerializedMonoBehaviour
    {
#pragma warning disable 649
        [SerializeField, InlineEditor] private IGenericValue<Texture2D> valueTexture;
#pragma warning restore 649

        public float AspectRatio => valueTexture.Value.width / (float)valueTexture.Value.height;
    }
}