using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class SatisfactionPresenter : MonoBehaviour
    {
        [SerializeField]
        Text text = null;

        private IPlayerOnegaiRepository playerOnegaiRepository;

        public void Initialize(IPlayerOnegaiRepository playerOnegaiRepository)
        {
            this.playerOnegaiRepository = playerOnegaiRepository;
        }

        private void Update()
        {
            SatisfactionCalculater satisfactionCalculater = new SatisfactionCalculater(playerOnegaiRepository);
            text.text = satisfactionCalculater.CalcFieldSatisfaction().ToString();
        }
    }
}


