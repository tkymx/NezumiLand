using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace NL
{
    public class ReserveAmountManager
    {
        private Dictionary<string, Dictionary<Type, IConsumableAmount >> valuePairs;

        public ReserveAmountManager()
        {
            this.valuePairs = new Dictionary<string, Dictionary<Type, IConsumableAmount>>();
        }

        public void Reserve<Amount>(string key, Amount amount) 
            where Amount : IConsumableAmount
        {
            if (!valuePairs.ContainsKey(key)) {
                valuePairs[key] = new Dictionary<Type, IConsumableAmount>();
            }

            var type = amount.GetType();
            if (!valuePairs[key].ContainsKey(type)) {
                valuePairs[key][type] = default(Amount);
            }

            valuePairs[key][type] = (Amount)valuePairs[key][type].Add_Implementation(amount);
        }

        public void UnReserve<Amount>(string key, Amount amount) 
            where Amount : IConsumableAmount
        {
            if (!valuePairs.ContainsKey(key)) {
                return;
            }

            var type = amount.GetType();
            if (!valuePairs[key].ContainsKey(type)) {
                return;
            }

            valuePairs[key][type] = (Amount)valuePairs[key][type].Subtraction_Implementation(amount);
        }

        private Type Type<T>() {
            T type = default(T);
            return type.GetType();
        }

        public Amount Get<Amount>() 
             where Amount : IConsumableAmount
       {
            var type = Type<Amount>();
            var amountList = this.valuePairs.Values
                .Where(typeToAmountDic => typeToAmountDic.ContainsKey(type))
                .Select(typeToAmountDic => typeToAmountDic[type])
                .ToList();

            if (amountList.Count <= 0) {
                return default(Amount);
            }
            var additionalAmount = (Amount)amountList[0];
            foreach (var amount in amountList)
            {
                additionalAmount.Add_Implementation(amount);
            }
            return additionalAmount;
        }

        public Amount GetByKey<Amount>(string key) {
            if(!this.valuePairs.ContainsKey(key)) {
                return default(Amount);
            }
            var type = Type<Amount>();
            if(!this.valuePairs[key].ContainsKey(type)) {
                return default(Amount);
            }
            return (Amount)this.valuePairs[key][type];
        }

        public void Reset (string key) {
            if (!this.valuePairs.ContainsKey(key)) {
                return;
            }

            var result = this.valuePairs.Remove(key);            
            Debug.Assert(result, "Resetが失敗しました。");
        }
    }   
}