using NUnit.Framework;
using Plugins.Clean.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Clean.Tests
{
    public class ComponentPropertiesTests
    {
        [Test]
        public void ComponentProperties_Text_CopiesText()
        {
            var go = new GameObject();
            var text = go.AddComponent<Text>();
            text.text = "testing123";
            var copy = new ComponentProperties.Text(text);
        
            Object.DestroyImmediate(text);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            copy.Apply(tmp);
        
            Assert.AreEqual("testing123", tmp.text);
        }
    }
}
