using CircularLinkedListVisualizer.Enums;
using CircularLinkedListVisualizer.Models;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CircularLinkedListVisualizer
{
    public partial class MainWindow : Window
    {
        private CircularLinkedList list = new CircularLinkedList();
        private OperationType currentOperation;
        private SubOperationType currentSubOperation;
        private Brush defaultNodeFill = Brushes.LightBlue;
        private Brush highlightedNodeFill = Brushes.Gold;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Operation_Checked(object sender, RoutedEventArgs e)
        {
            gbSubOperations.Visibility = Visibility.Visible;
            spSubOperations.Children.Clear();

            if (rbInsert.IsChecked == true)
            {
                currentOperation = OperationType.Insert;
                AddSubOperationRadioButtons(new[] { "InsertBack", "InsertFront", "InsertAt" });
            }
            else if (rbDelete.IsChecked == true)
            {
                currentOperation = OperationType.Delete;
                AddSubOperationRadioButtons(new[] { "RemoveFront", "RemoveBack", "RemoveAt" });
            }
            else if (rbUtilities.IsChecked == true)
            {
                currentOperation = OperationType.Utilities;
                AddSubOperationRadioButtons(new[] { "UpdateData", "Clear", "Search" });
            }
        }

        private void AddSubOperationRadioButtons(string[] operations)
        {
            foreach (var op in operations)
            {
                var rb = new RadioButton
                {
                    Content = op,
                    Margin = new Thickness(0, 5, 0, 5),
                    Tag = op
                };
                rb.Checked += (s, e) => SubOperation_Checked(rb.Tag.ToString());
                spSubOperations.Children.Add(rb);
            }
        }

        private void SubOperation_Checked(string operation)
        {
            currentSubOperation = (SubOperationType)Enum.Parse(typeof(SubOperationType), operation);
            UpdateInputFields();
        }

        private void UpdateInputFields()
        {
            txtData.IsEnabled = false;
            txtIndex.IsEnabled = false;
            btnExecute.IsEnabled = false;

            switch (currentSubOperation)
            {
                case SubOperationType.InsertBack:
                case SubOperationType.InsertFront:
                    txtData.IsEnabled = true;
                    btnExecute.IsEnabled = true;
                    break;

                case SubOperationType.InsertAt:
                case SubOperationType.UpdateData:
                case SubOperationType.RemoveAt:
                case SubOperationType.Search:
                    txtData.IsEnabled = currentSubOperation == SubOperationType.InsertAt ||
                                      currentSubOperation == SubOperationType.UpdateData;
                    txtIndex.IsEnabled = true;
                    btnExecute.IsEnabled = true;
                    break;

                case SubOperationType.RemoveFront:
                case SubOperationType.RemoveBack:
                case SubOperationType.Clear:
                    btnExecute.IsEnabled = true;
                    break;
            }
        }

        private async void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (currentSubOperation)
                {
                    case SubOperationType.InsertBack:
                        list.InsertBack(txtData.Text);
                        break;

                    case SubOperationType.InsertFront:
                        list.InsertFront(txtData.Text);
                        break;

                    case SubOperationType.InsertAt:
                        list.InsertAt(txtData.Text, int.Parse(txtIndex.Text));
                        break;

                    case SubOperationType.RemoveFront:
                        list.RemoveFront();
                        break;

                    case SubOperationType.RemoveBack:
                        list.RemoveBack();
                        break;

                    case SubOperationType.RemoveAt:
                        list.RemoveAt(int.Parse(txtIndex.Text));
                        break;

                    case SubOperationType.UpdateData:
                        list.UpdateData(int.Parse(txtIndex.Text), txtData.Text);
                        break;

                    case SubOperationType.Clear:
                        list.Clear();
                        break;

                    case SubOperationType.Search:
                        await AnimateSearch(int.Parse(txtIndex.Text));
                        return;
                }

                VisualizeList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VisualizeList()
        {
            canvasVisualization.Children.Clear();

            if (list.IsEmpty)
            {
                var text = new TextBlock
                {
                    Text = "Liste boş",
                    FontSize = 20,
                    Foreground = Brushes.Black
                };
                Canvas.SetLeft(text, canvasVisualization.ActualWidth / 2 - 30);
                Canvas.SetTop(text, canvasVisualization.ActualHeight / 2);
                canvasVisualization.Children.Add(text);
                return;
            }

            double centerX = canvasVisualization.ActualWidth / 2;
            double centerY = canvasVisualization.ActualHeight / 2;
            double radius = Math.Min(centerX, centerY) * 0.8;
            int nodeCount = list.Count;

            for (int i = 0; i < nodeCount; i++)
            {
                double angle = 2 * Math.PI * i / nodeCount;
                double x = centerX + radius * Math.Cos(angle);
                double y = centerY + radius * Math.Sin(angle);

                // Node circle
                var ellipse = new Ellipse
                {
                    Width = 50,
                    Height = 50,
                    Fill = defaultNodeFill,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Tag = i // Index bilgisini sakla
                };

                Canvas.SetLeft(ellipse, x - 25);
                Canvas.SetTop(ellipse, y - 25);
                canvasVisualization.Children.Add(ellipse);

                // Node data text
                var text = new TextBlock
                {
                    Text = list.GetDataAt(i),
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Tag = i
                };

                Canvas.SetLeft(text, x - 15);
                Canvas.SetTop(text, y - 10);
                canvasVisualization.Children.Add(text);

                // Arrow to next node
                if (i < nodeCount - 1)
                {
                    DrawArrow(x, y, angle, nodeCount);
                }
            }

            // Son düğümden ilk düğüme ok çiz
            if (nodeCount > 1)
            {
                double lastAngle = 2 * Math.PI * (nodeCount - 1) / nodeCount;
                double lastX = centerX + radius * Math.Cos(lastAngle);
                double lastY = centerY + radius * Math.Sin(lastAngle);
                DrawArrow(lastX, lastY, lastAngle, nodeCount);
            }
        }

        private void DrawArrow(double startX, double startY, double angle, int nodeCount)
        {
            double centerX = canvasVisualization.ActualWidth / 2;
            double centerY = canvasVisualization.ActualHeight / 2;
            double radius = Math.Min(centerX, centerY) * 0.8;

            double nextAngle = angle + 2 * Math.PI / nodeCount;
            double endX = centerX + radius * Math.Cos(nextAngle);
            double endY = centerY + radius * Math.Sin(nextAngle);

            // Ok çizgisi
            var line = new Line
            {
                X1 = startX + 25 * Math.Cos(angle),
                Y1 = startY + 25 * Math.Sin(angle),
                X2 = endX - 25 * Math.Cos(nextAngle),
                Y2 = endY - 25 * Math.Sin(nextAngle),
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            canvasVisualization.Children.Add(line);

            // Ok başı
            DrawArrowHead(line.X1, line.Y1, line.X2, line.Y2);
        }

        private void DrawArrowHead(double x1, double y1, double x2, double y2)
        {
            double arrowSize = 10;
            double angle = Math.Atan2(y2 - y1, x2 - x1);

            var arrow = new Polygon
            {
                Points = new PointCollection
                {
                    new Point(x2, y2),
                    new Point(x2 - arrowSize * Math.Cos(angle - Math.PI/6), y2 - arrowSize * Math.Sin(angle - Math.PI/6)),
                    new Point(x2 - arrowSize * Math.Cos(angle + Math.PI/6), y2 - arrowSize * Math.Sin(angle + Math.PI/6))
                },
                Fill = Brushes.Black
            };
            canvasVisualization.Children.Add(arrow);
        }

        private async Task AnimateSearch(int targetIndex)
        {
            if (list.IsEmpty || targetIndex < 0 || targetIndex >= list.Count)
            {
                MessageBox.Show("Geçersiz index!", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            for (int i = 0; i <= targetIndex; i++)
            {
                HighlightNode(i);
                await Task.Delay(500);

                if (i < targetIndex)
                {
                    UnhighlightNode(i);
                }
            }
        }

        private void HighlightNode(int index)
        {
            foreach (var child in canvasVisualization.Children)
            {
                if (child is Ellipse ellipse && ellipse.Tag is int i && i == index)
                {
                    ellipse.Fill = highlightedNodeFill;
                }
            }
        }

        private void UnhighlightNode(int index)
        {
            foreach (var child in canvasVisualization.Children)
            {
                if (child is Ellipse ellipse && ellipse.Tag is int i && i == index)
                {
                    ellipse.Fill = defaultNodeFill;
                }
            }
        }
    }
}