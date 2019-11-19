using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class SatisfactionCalculater
    {
        private readonly IPlayerOnegaiRepository playerOnegaiRepository;

        public SatisfactionCalculater(IPlayerOnegaiRepository playerOnegaiRepository)
        {
            this.playerOnegaiRepository = playerOnegaiRepository;
        }

        public Satisfaction CalcFieldSatisfaction()
        {
            Satisfaction satisfaction = new Satisfaction(0);
            satisfaction += GameManager.Instance.MonoManager.GetAllSatisfaction();
            satisfaction += playerOnegaiRepository.GetAllSatisfaction();
            return satisfaction;
        }
    }
}

