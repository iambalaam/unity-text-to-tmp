using NUnit.Framework;
using Plugins.Clean.Editor;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Clean.Tests
{
    public class UnityEventTests
    {
        [Test]
        public void CanCopyUnityEvent()
        {
            bool hasBeenChanged = false;

            // Add listener to input1
            var go1 = new GameObject();
            var input1 = go1.AddComponent<InputField>();
            input1.onValueChanged.AddListener(_ => { hasBeenChanged = true; });

            // Copy event to input2
            var go2 = new GameObject();
            var input2 = go2.AddComponent<InputField>();
            var onChanged = Util.CopyUnityEvent(input1.onValueChanged);
            Util.PasteUnityEvent(input2.onValueChanged, onChanged);

            Assert.IsFalse(hasBeenChanged);
            input2.text = "a new value";
            Assert.IsTrue(hasBeenChanged);
        }

    }
}
