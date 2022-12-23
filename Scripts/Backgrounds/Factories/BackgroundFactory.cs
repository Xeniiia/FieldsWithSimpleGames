using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Games.InCircles.Scripts.Backgrounds.Factories
{
    public class BackgroundFactory : MonoBehaviour
    {
        [SerializeField] private GameObject[] backgroundPrefabs;
        private List<GameObject> _prefabs;


        private void Awake()
        {
            _prefabs = backgroundPrefabs.ToList();
        }

        public GameObject GetBackground()
        {
            if (_prefabs.Count == 0)
                
            {
                _prefabs = backgroundPrefabs.ToList();
            }
            
            var rnd = Random.Range(0, _prefabs.Count);
            var background =  _prefabs[rnd];
            _prefabs.RemoveAt(rnd);

            return background;
        }
    }
}