using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MONOGAME_VERSION_5
{
    internal static class WeightedProbability
    {
        public static T GetRandomWeightedItem<T>(List<(T item, int weight)> items)
        {
            int totalWeight = items.Sum(i => i.weight);
            Random random = new Random();
            int randomWeight = random.Next(totalWeight);

            int currentWeight = 0;
            foreach (var item in items)
            {
                currentWeight += item.weight;
                if (randomWeight < currentWeight)
                {
                    return item.item;
                }
            }

            // Fallback, should not reach here if weights are correct
            return items[0].item;
        }
    }
}
