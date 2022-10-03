using System.Collections.Generic;
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

        [Test]
        public void ComponentProperties_Dropdown_CopiesOptions()
        {
            var go1 = new GameObject();
            var dropdown = go1.AddComponent<Dropdown>();
            dropdown.options = new List<Dropdown.OptionData>
            {
                new("option 1"),
                new("option 2")
            };
            var properties = new ComponentProperties.Dropdown(dropdown);

            var go2 = new GameObject();
            var tmpDropdown = go2.AddComponent<TMP_Dropdown>();
            properties.Apply(tmpDropdown);

            Assert.AreEqual(2, tmpDropdown.options.Count, "Failed to copy all options");
            Assert.AreEqual("option 1", tmpDropdown.options[0].text, "Failed to copy first option");
            Assert.AreEqual("option 2", tmpDropdown.options[1].text, "Failed to copy second option");
        }
    }
}
