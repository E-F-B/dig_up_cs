using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TGJ2021.InGame.Result
{
    [CreateAssetMenu]
    public class AdviceSetting : ScriptableObject
    {
        [SerializeField] 
        [TableList(ShowIndexLabels = true)]
        private List<Advice> _advices;

        public List<Advice> Advices => _advices;
    }

    [Serializable]
    public class Advice
    {
        [TableList(ShowIndexLabels = true)]
        [PreviewField(ObjectFieldAlignment.Center)]
        public Sprite _sprite;

        [TextArea] public string _adviceText;
    }
}