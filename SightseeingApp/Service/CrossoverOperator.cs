using GAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SightseeingApp.Service
{
    public class CrossoverOperator :IGeneticOperator
    {
        public void Invoke(Population currentPopulation,
                               ref Population newPopulation,
                               FitnessFunction fitnesFunctionDelegate)
        {
            throw new NotImplementedException();
        }

        public int GetOperatorInvokedEvaluations()
        {
            throw new NotImplementedException();
        }

        public bool Enabled { get; set; }
        public bool RequiresEvaluatedPopulation { get; set; }
    }
}