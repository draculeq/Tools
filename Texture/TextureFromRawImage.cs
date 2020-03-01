using System;
using Deadbit.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace Deadbit.Tools
{
    [Serializable]
    public class TextureFromRawImage : IGenericValue<Texture2D>
    {
#pragma warning disable 649
        [SerializeField] private IGenericVariable<RawImage> rawImage;
#pragma warning restore 649

        public Texture2D Value => (Texture2D) rawImage?.Value.mainTexture;
    }
}