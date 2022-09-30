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
        public void ComponentProperties_Selectable_InputField()
        {
            var button = new GameObject().AddComponent<Button>();
            button.transition = Selectable.Transition.Animation;
            var copy = new ComponentProperties.SelectableObject(button);
            
            Assert.AreEqual(button.transition, Selectable.Transition.Animation);
        }
        
        [Test]
        public void ComponentProperties_Text_CopiesText()
        {
            var go1 = new GameObject();
            var text = go1.AddComponent<Text>();
            text.text = "testing123";
            var copy = new ComponentProperties.Text(text);

            var go2 = new GameObject();
            var tmp = go2.AddComponent<TextMeshProUGUI>();
            copy.Apply(tmp);
        
            Assert.AreEqual("testing123", tmp.text);
        }
    }
}
