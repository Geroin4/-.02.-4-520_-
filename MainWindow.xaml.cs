using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ПМ02_4ИСИП520_Крушеницкий
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TransportZadacha(object sender, RoutedEventArgs e)
        {
            string[] demandInputs = txtConsumerDemand.Text.Split(' ');
            string[] supplyInputs = txtSupplierSupply.Text.Split(' ');
            string[] costRows = txtCostMatrix.Text.Split(':');
            int[] demand = Array.ConvertAll(demandInputs, int.Parse);
            int[] supply = Array.ConvertAll(supplyInputs, int.Parse);
            int[][] costMatrix = new int[costRows.Length][];
            for (int i = 0; i < costRows.Length; i++)
            {
                costMatrix[i] = costRows[i].Split(' ').Select(int.Parse).ToArray();
            }
            


            var (allocation, totalCost) = MinimumCostMethod(supply, demand, costMatrix);
            DisplayResult(allocation, totalCost);
        }
        static (int[][], int) MinimumCostMethod(int[] supply, int[] demand, int[][] costs)
        {
            int[][] allocation = new int[supply.Length][];
            for (int i = 0; i < supply.Length; i++)
            {
                allocation[i] = new int[demand.Length];
            }

            int[] supplyCopy = supply.ToArray();
            int[] demandCopy = demand.ToArray();
            int totalCost = 0;

            while (true)
            {
                int minCost = int.MaxValue;
                int minRow = -1, minCol = -1;

                for (int row = 0; row < supply.Length; row++)
                {
                    for (int col = 0; col < demand.Length; col++)
                    {
                        if (supplyCopy[row] > 0 && demandCopy[col] > 0)
                        {
                            if (costs[row][col] < minCost)
                            {
                                minCost = costs[row][col];
                                minRow = row;
                                minCol = col;
                            }
                        }
                    }
                }

                if (minRow == -1 || minCol == -1)
                {
                    break;
                }

                int x = Math.Min(supplyCopy[minRow], demandCopy[minCol]);
                allocation[minRow][minCol] = x;
                supplyCopy[minRow] -= x;
                demandCopy[minCol] -= x;
                totalCost += x * minCost;
            }


            return (allocation, totalCost);
        }

        private void DisplayResult(int[][] allocation, int totalCost)
        {
            StringBuilder resultBuilder = new StringBuilder();

            
            resultBuilder.AppendLine("Опорный план:");
            for (int i = 0; i < allocation.Length; i++)
            {
                for (int j = 0; j < allocation[i].Length; j++)
                {
                    resultBuilder.Append(allocation[i][j]);
                    resultBuilder.Append("\t");
                }
                resultBuilder.AppendLine();
            }
            resultBuilder.AppendLine($"Общая стоимость: {totalCost}");

            txtSolution.Text = resultBuilder.ToString();
        }
    }
}
