using System;
using System.Linq;
using RoyalMasion.Code.Infrastructure.Attributes;
using UnityEngine;
using VContainer.Unity;

namespace RoyalMasion.Code.Infrastructure.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void AutoInjectAll()
        {
            base.AutoInjectAll();

            var gameObjects = FindObjectsOfType<GameObject>();
            foreach (var gameObjectInstance in gameObjects)
            {
                var components = gameObjectInstance.GetComponents<MonoBehaviour>()
                    .Where(x => Attribute.IsDefined(x.GetType(), typeof(AutoInjectAttribute)));
                if (gameObjectInstance != null && components.Any())
                {
                    Container.InjectGameObject(gameObjectInstance);
                }
            }
        }
    }
}
