using NUnit.Framework;
using Plugins.Clean.Editor;
using UnityEngine;

namespace Plugins.Clean.Tests
{
    public class UtilTests
    {
        [Test]
        public void RecurseOverChildren_SingleObject()
        {
            var count = 0;
            var go = new GameObject();
            Util.RecurseOverChildren(go, (_) => { count++;});

            Assert.AreEqual(1, count);
        }

        [Test]
        public void RecurseOverChildren_NestedObjects()
        {
            var count = 0;
            var grandparent = new GameObject();
            var parent1 = new GameObject();
            parent1.transform.SetParent(grandparent.transform);
            var parent2 = new GameObject();
            parent2.transform.SetParent(grandparent.transform);
            var child = new GameObject();
            child.transform.SetParent(parent1.transform);
            Util.RecurseOverChildren(grandparent, (_) => { count++;});

            Assert.AreEqual(4, count);
        }
    }
}
