using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Xml;
using WpfApp1.Model;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool elipseChecked = false;
        private bool textChecked = false;
        private bool polygonChecked = false;

        public string text;
        public string textColor;
        public double textSize = 10;
        public int elipseIndex = 0;
        public int polygoneIndex = 0;
        public int textIndex = 0;

        public Image newImage;
        public TextBlock newTextBlock;

        private static int zIndex = 0;

        private Dictionary<string, List<Image>> images = new Dictionary<string, List<Image>>();
        private Dictionary<string, List<TextBlock>> textBlocks = new Dictionary<string, List<TextBlock>>();
        private List<Image> allActiveImages = new List<Image>();
        private List<TextBlock> allActiveTexts = new List<TextBlock>();

        private Dictionary<Image, TextBlock> pairs = new Dictionary<Image, TextBlock>();

        private List<Tuple<string, Image>> undo = new List<Tuple<string, Image>>();
        private List<Tuple<string, Image>> redo = new List<Tuple<string, Image>>();

        private List<Tuple<string, TextBlock>> undoText = new List<Tuple<string, TextBlock>>();
        private List<Tuple<string, TextBlock>> redoText = new List<Tuple<string, TextBlock>>();


        public PointCollection polygonPoints = new PointCollection();



        public double noviX, noviY;
        private DB db = new DB();


        public bool ElipseChecked { get => elipseChecked; set { if (elipseChecked != value) { elipseChecked = value; OnPropertyChanged("ElipseChecked"); }; } }
        public bool PolygonChecked { get => polygonChecked; set { if (polygonChecked != value) { polygonChecked = value; OnPropertyChanged("PolygonChecked"); }; } }
        public bool TextChecked { get => textChecked; set { if (textChecked != value) { textChecked = value; OnPropertyChanged("TextChecked"); }; } }


        public MainWindow()
        {
            InitializeComponent();
            var doc = new XmlDocument();
            doc.Load("Geographic.xml");
            db.InititalizeDataBase(doc);
            db.SetToCanvasScale(drawing_canvas.Width, drawing_canvas.Height);
            db.SetCoordinates(drawing_canvas.Width, drawing_canvas.Height);
            db.Draw(drawing_canvas);


            images.Add("elipse", new List<Image>());
            images.Add("polygon", new List<Image>());
            images.Add("text", new List<Image>());

            textBlocks.Add("elipse", new List<TextBlock>());
            textBlocks.Add("polygon", new List<TextBlock>());
            textBlocks.Add("text", new List<TextBlock>());
            this.DataContext = this;


        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }



        private void ElipseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ElipseChecked == true)
            {
                ElipseChecked = false;
            }
            else
            {
                ElipseChecked = true;
            }

            PolygonChecked = false;
            TextChecked = false;
        }

        private void PolygonBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PolygonChecked == true)
            {
                PolygonChecked = false;
                deletePolygonPoints();

                polygonPoints.Clear();

            }
            else
            {
                PolygonChecked = true;

            }

            ElipseChecked = false;
            TextChecked = false;
        }

        private void deletePolygonPoints()
        {
            int i = 0;
            int j = 0;
            for (i = drawing_canvas.Children.Count - 1, j = polygonPoints.Count; i >= 0 && j > 0; i--, j--)
            {
                drawing_canvas.Children.RemoveAt(i);
            }
        }

        private void TextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TextChecked == true)
            {
                TextChecked = false;
            }
            else
            {
                TextChecked = true;
            }

            PolygonChecked = false;
            ElipseChecked = false;
        }

        private void UndoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (undo.Count != 0)
            {
                deletePolygonPoints();
                polygonPoints.Clear();
                switch (undo[undo.Count - 1].Item1)
                {
                    case "remove":
                        Tuple<string, Image> toRemove = undo[undo.Count - 1];
                        Tuple<string, TextBlock> toRemoveText = undoText[undoText.Count - 1];

                        drawing_canvas.Children.Remove(toRemove.Item2);
                        drawing_canvas.Children.Remove(toRemoveText.Item2);
                        redo.Add(new Tuple<string, Image>("add", toRemove.Item2));
                        redoText.Add(new Tuple<string, TextBlock>("add", toRemoveText.Item2));
                        undo.Remove(toRemove);
                        undoText.Remove(toRemoveText);
                        allActiveImages.Remove(toRemove.Item2);
                        allActiveTexts.Remove(toRemoveText.Item2);

                        break;
                    case "return":
                        for (int i = undo.Count - 1; i >= 0; i--)
                        {
                            Tuple<string, Image> toReturn = undo[i];
                            Tuple<string, TextBlock> toReturnText = undoText[i];
                            if (toReturn.Item1 != "return")
                                break;
                            drawing_canvas.Children.Add(toReturn.Item2);
                            drawing_canvas.Children.Add(toReturnText.Item2);
                            undo.Remove(toReturn);
                            undoText.Remove(toReturnText);
                            redo.Add(new Tuple<string, Image>("removeAll", toReturn.Item2));
                            redoText.Add(new Tuple<string, TextBlock>("removeAll", toReturnText.Item2));
                            allActiveImages.Add(toReturn.Item2);
                            allActiveTexts.Add(toReturnText.Item2);
                        }

                        break;
                    case "replace":
                        Tuple<string, Image> toReplace = undo[undo.Count - 1];
                        Tuple<string, TextBlock> toReplaceText = undoText[undoText.Count - 1];
                        drawing_canvas.Children.Remove(toReplace.Item2);
                        drawing_canvas.Children.Remove(toReplaceText.Item2);
                        undo.Remove(toReplace);
                        undoText.Remove(toReplaceText);
                        redo.Add(new Tuple<string, Image>("replace", toReplace.Item2));
                        redoText.Add(new Tuple<string, TextBlock>("replace", toReplaceText.Item2));
                        allActiveImages.Remove(toReplace.Item2);
                        allActiveTexts.Remove(toReplaceText.Item2);

                        toReplace = undo[undo.Count - 1];
                        toReplaceText = undoText[undoText.Count - 1];
                        drawing_canvas.Children.Add(toReplace.Item2);
                        drawing_canvas.Children.Add(toReplaceText.Item2);
                        undo.Remove(toReplace);
                        undoText.Remove(toReplaceText);
                        redo.Add(new Tuple<string, Image>("replace", toReplace.Item2));
                        redoText.Add(new Tuple<string, TextBlock>("replace", toReplaceText.Item2));
                        allActiveImages.Add(toReplace.Item2);
                        allActiveTexts.Add(toReplaceText.Item2);

                        break;
                    default:
                        break;
                }
            }
        }

        private void RedoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (redo.Count != 0)
            {
                deletePolygonPoints();
                polygonPoints.Clear();
                switch (redo[redo.Count - 1].Item1)
                {
                    case "add":
                        Tuple<string, Image> toAdd = redo[redo.Count - 1];
                        Tuple<string, TextBlock> toAddText = redoText[redoText.Count - 1];

                        drawing_canvas.Children.Add(toAdd.Item2);
                        drawing_canvas.Children.Add(toAddText.Item2);

                        undo.Add(new Tuple<string, Image>("remove", toAdd.Item2));
                        undoText.Add(new Tuple<string, TextBlock>("remove", toAddText.Item2));

                        redo.Remove(toAdd);
                        redoText.Remove(toAddText);

                        allActiveImages.Add(toAdd.Item2);
                        allActiveTexts.Add(toAddText.Item2);

                        break;
                    case "removeAll":
                        for (int i = redo.Count - 1; i >= 0; i--)
                        {
                            if (redo[i].Item1 != "removeAll")
                                break;

                            Tuple<string, Image> toRemove = redo[i];
                            Tuple<string, TextBlock> toRemoveText = redoText[i];

                            drawing_canvas.Children.Remove(toRemove.Item2);
                            drawing_canvas.Children.Remove(toRemoveText.Item2);

                            undo.Add(new Tuple<string, Image>("return", toRemove.Item2));
                            undoText.Add(new Tuple<string, TextBlock>("return", toRemoveText.Item2));

                            redo.Remove(toRemove);
                            redoText.Remove(toRemoveText);

                            allActiveImages.Remove(toRemove.Item2);
                            allActiveTexts.Remove(toRemoveText.Item2);


                        }

                        break;
                    case "replace":
                        Tuple<string, Image> toReplace = redo[redo.Count - 1];
                        Tuple<string, TextBlock> toReplaceText = redoText[redoText.Count - 1];

                        drawing_canvas.Children.Remove(toReplace.Item2);
                        drawing_canvas.Children.Remove(toReplaceText.Item2);

                        redo.Remove(toReplace);
                        redoText.Remove(toReplaceText);

                        undo.Add(new Tuple<string, Image>("replace", toReplace.Item2));
                        undoText.Add(new Tuple<string, TextBlock>("replace", toReplaceText.Item2));

                        allActiveImages.Remove(toReplace.Item2);
                        allActiveTexts.Remove(toReplaceText.Item2);


                        toReplace = redo[redo.Count - 1];
                        toReplaceText = redoText[redoText.Count - 1];

                        drawing_canvas.Children.Add(toReplace.Item2);
                        drawing_canvas.Children.Add(toReplaceText.Item2);

                        redo.Remove(toReplace);
                        redoText.Remove(toReplaceText);

                        undo.Add(new Tuple<string, Image>("replace", toReplace.Item2));
                        undoText.Add(new Tuple<string, TextBlock>("replace", toReplaceText.Item2));

                        allActiveImages.Add(toReplace.Item2);
                        allActiveTexts.Add(toReplaceText.Item2);


                        break;
                    default:
                        break;
                }
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Image img in allActiveImages)
            {
                undo.Add(new Tuple<string, Image>("return", img));

            }
            foreach (TextBlock img in allActiveTexts)
            {
                undoText.Add(new Tuple<string, TextBlock>("return", img));

            }

            allActiveImages.RemoveRange(0, allActiveImages.Count);
            allActiveTexts.RemoveRange(0, allActiveTexts.Count);

            redo.RemoveRange(0, redo.Count);
            redoText.RemoveRange(0, redoText.Count);

            polygonPoints.Clear();
            ClearCanvas();
        }

        private void ClearCanvas()
        {
            drawing_canvas.Children.RemoveRange(0, drawing_canvas.Children.Count);
        }

        private void Mouse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (polygonPoints.Count < 3)
            {
                deletePolygonPoints();
                polygonPoints.Clear();


                if (e.OriginalSource is Image)
                {
                    Image reworkImage = (Image)e.OriginalSource;

                    for (int i = 0; i < drawing_canvas.Children.Count; i++)
                    {
                        if (drawing_canvas.Children[i] == reworkImage)
                        {
                            string objType;
                            if (images["elipse"].Contains(reworkImage))
                            {

                                objType = "elipse";
                                DrawingImage di = new DrawingImage();
                                di = (DrawingImage)reworkImage.Source;

                                GeometryDrawing gd = new GeometryDrawing();
                                gd = (GeometryDrawing)di.Drawing;

                                Pen p = new Pen();
                                p = gd.Pen;

                                EllipseGeometry eg = new EllipseGeometry();
                                eg = (EllipseGeometry)gd.Geometry;

                                var ed = new ElipseDrawing();

                                ed.radiusX.Text = eg.RadiusX.ToString();
                                ed.radiusY.Text = eg.RadiusY.ToString();
                                ed.borderSize.Text = p.Thickness.ToString();
                                ed.text.Text = text;

                                SolidColorBrush borderScb = null;
                                SolidColorBrush fillScb = null;

                                SolidColorBrush textBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(textColor);
                                ed.textColorPicker.SelectedItem = textBrush;

                                foreach (var item in ed.borderColorPicker.ItemsSource)
                                {
                                    if ((borderScb = (SolidColorBrush)new BrushConverter().ConvertFromString(item.ToString().Split(' ')[1])) == p.Brush)
                                    {
                                        ed.borderColorPicker.SelectedItem = item;
                                        break;
                                    }
                                }

                                foreach (var item in ed.fillColorPicker.ItemsSource)
                                {
                                    if ((fillScb = (SolidColorBrush)new BrushConverter().ConvertFromString(item.ToString().Split(' ')[1])) == gd.Brush)
                                    {
                                        ed.fillColorPicker.SelectedItem = item;
                                        break;
                                    }
                                }

                                ed.ShowDialog();
                                SolidColorBrush newBorderScb = (SolidColorBrush)new BrushConverter().ConvertFromString(ed.borderColorPicker.SelectedItem.ToString().Split(' ')[1]);
                                SolidColorBrush newFillScb = (SolidColorBrush)new BrushConverter().ConvertFromString(ed.fillColorPicker.SelectedItem.ToString().Split(' ')[1]);
                                SolidColorBrush newTextScb = (SolidColorBrush)new BrushConverter().ConvertFromString(ed.textColorPicker.SelectedItem.ToString().Split(' ')[1]);


                                if (ed.radiusX.Text == eg.RadiusX.ToString() &&
                                    ed.radiusY.Text == eg.RadiusY.ToString() &&
                                    ed.borderSize.Text == p.Thickness.ToString() &&
                                    ed.text.Text == text &&
                                    newBorderScb == borderScb &&
                                    newFillScb == fillScb &&
                                    newTextScb == textBrush
                                    )
                                {
                                    break;
                                }

                            }

                            else if (images["polygon"].Contains(reworkImage))
                            {
                                objType = "polygon";
                                DrawingImage di = new DrawingImage();
                                di = (DrawingImage)reworkImage.Source;

                                GeometryDrawing gd = new GeometryDrawing();
                                gd = (GeometryDrawing)di.Drawing;

                                Pen p = new Pen();
                                p = gd.Pen;

                                PathGeometry pg = new PathGeometry();
                                pg = (PathGeometry)gd.Geometry;

                                PathFigureCollection pfc = new PathFigureCollection();
                                pfc = pg.Figures;

                                polygonPoints.Add(pfc[0].StartPoint);
                                foreach (var item in pfc[0].Segments)
                                {
                                    LineSegment ls = (LineSegment)item;
                                    polygonPoints.Add(ls.Point);
                                }

                                var pd = new PolygonDrawing();

                                pd.borderSize.Text = p.Thickness.ToString();
                                pd.text.Text = text;
                                SolidColorBrush textBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(textColor);
                                pd.textColorPicker.SelectedItem = textBrush;

                                SolidColorBrush borderScb = null;
                                SolidColorBrush fillScb = null;

                                foreach (var item in pd.borderColorPicker.ItemsSource)
                                {
                                    if ((borderScb = (SolidColorBrush)new BrushConverter().ConvertFromString(item.ToString().Split(' ')[1])) == p.Brush)
                                    {
                                        pd.borderColorPicker.SelectedItem = item;
                                        break;
                                    }
                                }

                                foreach (var item in pd.fillColorPicker.ItemsSource)
                                {
                                    if ((fillScb = (SolidColorBrush)new BrushConverter().ConvertFromString(item.ToString().Split(' ')[1])) == gd.Brush)
                                    {
                                        pd.fillColorPicker.SelectedItem = item;
                                        break;
                                    }
                                }

                                pd.ShowDialog();
                                SolidColorBrush newBorderScb = (SolidColorBrush)new BrushConverter().ConvertFromString(pd.borderColorPicker.SelectedItem.ToString().Split(' ')[1]);
                                SolidColorBrush newFillScb = (SolidColorBrush)new BrushConverter().ConvertFromString(pd.fillColorPicker.SelectedItem.ToString().Split(' ')[1]);
                                SolidColorBrush newTextScb = (SolidColorBrush)new BrushConverter().ConvertFromString(pd.textColorPicker.SelectedItem.ToString().Split(' ')[1]);

                                polygonPoints.Clear();

                                if (pd.borderSize.Text == p.Thickness.ToString() &&
                                    newBorderScb == borderScb &&
                                    pd.text.Text == text &&
                                    newTextScb == textBrush &&
                                    newFillScb == fillScb)
                                {
                                    break;
                                }

                            }
                            else if (images["text"].Contains(reworkImage))
                            {
                                objType = "text";

                                var td = new TextDrawing();

                                td.text.Text = text;
                                td.textSize.Text = textSize.ToString();


                                SolidColorBrush textBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(textColor);
                                td.textColorPicker.SelectedItem = textBrush;

                                td.ShowDialog();

                                SolidColorBrush newTextScb = (SolidColorBrush)new BrushConverter().ConvertFromString(td.textColorPicker.SelectedItem.ToString().Split(' ')[1]);


                                if (
                                    td.text.Text == text &&
                                    td.textSize.Text == textSize.ToString() &&
                                    newTextScb == textBrush
                                    )
                                {
                                    break;
                                }

                            }
                            else
                            {
                                break;
                            }

                            if (newImage != null)
                            {

                                TextBlock value = null; ;
                                pairs.TryGetValue(reworkImage, out value);
                                images[objType].Remove(reworkImage);
                                textBlocks[objType].Remove(value);
                                allActiveImages.Remove(reworkImage);
                                allActiveTexts.Remove(value);





                                TextBlock textBlock = new TextBlock();
                                textBlock.Text = text;
                                SolidColorBrush textBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(textColor);
                                textBlock.Foreground = textBrush;
                                textBlock.FontSize = textSize;


                                int newIndex = 0;
                                double mouseX = Canvas.GetLeft(reworkImage);
                                double mouseY = Canvas.GetTop(reworkImage);

                                for(int j=0; j<drawing_canvas.Children.Count; j++)
                                {
                                    if (drawing_canvas.Children[j] == value)
                                    {
                                        newIndex = j;
                                    }
                                }

                               
                                //    if (objType == "polygon")
                                //{
                                //    newIndex = polygoneIndex - 2;
                                //}
                                //else if (objType == "elipse")
                                //{
                                //    newIndex = elipseIndex - 2;
                                //}
                                //else if (objType == "text")
                                //{
                                //    newIndex = textIndex - 2;
                                //}

                                drawing_canvas.Children.RemoveAt(i);

                                UIElement uiel = value;

                                drawing_canvas.Children.Remove(uiel);

                                Canvas.SetLeft(newImage, mouseX);
                                Canvas.SetTop(newImage, mouseY);
                                Canvas.SetLeft(textBlock, mouseX+20);
                                Canvas.SetTop(textBlock, mouseY+20);
                                Panel.SetZIndex(newImage, Panel.GetZIndex(reworkImage));
                                int value1 = Panel.GetZIndex(reworkImage) + 1;
                                Panel.SetZIndex(textBlock, value1);

                                drawing_canvas.Children.Add(newImage);
                                drawing_canvas.Children.Add(textBlock);
                                images[objType].Add(newImage);

                                textBlocks[objType].Add(textBlock);
                                allActiveImages.Add(newImage);
                                allActiveTexts.Add(textBlock);

                                undo.Add(new Tuple<string, Image>("replace", reworkImage));
                                undo.Add(new Tuple<string, Image>("replace", newImage));

                                undoText.Add(new Tuple<string, TextBlock>("replace", value));
                                undoText.Add(new Tuple<string, TextBlock>("replace", textBlock));


                                redo.RemoveRange(0, redo.Count);
                                redoText.RemoveRange(0, redoText.Count);
                                pairs.Add(newImage, textBlock);

                                newImage = null;
                            }
                            break;
                        }
                    }
                }

               

                else
                {
                    if (PolygonChecked)
                    {

                        deletePolygonPoints();
                    }


                }
            }
            else
            {
                var pd = new PolygonDrawing();
                pd.ShowDialog();

                if (newImage != null)
                {
                    double minX = polygonPoints[0].X;
                    double minY = polygonPoints[0].Y;
                    foreach (System.Windows.Point p in polygonPoints)
                    {
                        if (p.X < minX)
                        {
                            minX = p.X;
                        }
                        if (p.Y < minY)
                        {
                            minY = p.Y;
                        }
                    }


                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = text;
                    SolidColorBrush textBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(textColor);
                    textBlock.Foreground = textBrush;

                    Canvas.SetLeft(textBlock, minX+30);
                    Canvas.SetTop(textBlock, minY+40);


                    Canvas.SetLeft(newImage, minX);
                    Canvas.SetTop(newImage, minY);

                    Panel.SetZIndex(newImage, ++zIndex);
                    Panel.SetZIndex(textBlock, ++zIndex);

                    deletePolygonPoints();

                    drawing_canvas.Children.Add(newImage);
                    drawing_canvas.Children.Add(textBlock);
                    polygoneIndex = drawing_canvas.Children.Count;

                    images["polygon"].Add(newImage);
                    textBlocks["polygon"].Add(textBlock);

                    allActiveImages.Add(newImage);
                    allActiveTexts.Add(textBlock);
                    undo.Add(new Tuple<string, Image>("remove", newImage));
                    undoText.Add(new Tuple<string, TextBlock>("remove", textBlock));

                    polygonPoints.Clear();

                    redo.RemoveRange(0, redo.Count);
                    redoText.RemoveRange(0, redoText.Count);

                    pairs.Add(newImage, textBlock);

                    newImage = null;
                    newTextBlock = null;


                }

            }
        }
    

        private void Mouse_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ElipseChecked)
            {
                double mouseX = Mouse.GetPosition(drawing_canvas).X;
                double mouseY = Mouse.GetPosition(drawing_canvas).Y;

                var ed = new ElipseDrawing();
                ed.ShowDialog();

                if (newImage != null)
                {


                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = text;
                    SolidColorBrush textBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(textColor);
                    textBlock.Foreground= textBrush;


                    Canvas.SetLeft(newImage, mouseX);
                    Canvas.SetTop(newImage, mouseY);
                    Canvas.SetLeft(textBlock, mouseX+20);
                    Canvas.SetTop(textBlock, mouseY+20);


                    Panel.SetZIndex(newImage, ++zIndex);
                    Panel.SetZIndex(textBlock, ++zIndex);

                    drawing_canvas.Children.Add(newImage);
                    drawing_canvas.Children.Add(textBlock);
                    elipseIndex=drawing_canvas.Children.Count;
                    images["elipse"].Add(newImage);
                    textBlocks["elipse"].Add(textBlock);

                    
                    allActiveImages.Add(newImage);
                    allActiveTexts.Add(textBlock);
                    
                    undo.Add(new Tuple<string, Image>("remove", newImage));
                    undoText.Add(new Tuple<string, TextBlock>("remove", textBlock));

                    redo.RemoveRange(0, redo.Count);
                    redoText.RemoveRange(0, redoText.Count);

                    pairs.Add(newImage, textBlock);

                    newImage = null;
                    newTextBlock = null;
                   // textImage = null;
                }
            }
            else if (PolygonChecked)
            {

                polygonPoints.Add(Mouse.GetPosition(drawing_canvas));
            
                Ellipse ell = new Ellipse() { Width = 3, Height = 3, Fill = Brushes.Black };
                Canvas.SetLeft(ell, Mouse.GetPosition(drawing_canvas).X);
                Canvas.SetTop(ell, Mouse.GetPosition(drawing_canvas).Y);
                drawing_canvas.Children.Add(ell);
            }
            else if (TextChecked)
            {
                double mouseX = Mouse.GetPosition(drawing_canvas).X;
                double mouseY = Mouse.GetPosition(drawing_canvas).Y;

                var td = new TextDrawing();
                td.ShowDialog();

                if (newImage != null)
                {


                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = text;
                    SolidColorBrush textBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(textColor);
                    textBlock.Foreground = textBrush;
                    textBlock.FontSize = textSize;
                    
                    Canvas.SetLeft(newImage, mouseX);
                    Canvas.SetTop(newImage, mouseY);
                    Canvas.SetLeft(textBlock, mouseX);
                    Canvas.SetTop(textBlock, mouseY);


                    Panel.SetZIndex(newImage, ++zIndex);
                    Panel.SetZIndex(textBlock, ++zIndex);

                    drawing_canvas.Children.Add(newImage);
                    drawing_canvas.Children.Add(textBlock);
                    textIndex = drawing_canvas.Children.Count;

                    images["text"].Add(newImage);
                    textBlocks["text"].Add(textBlock);

                    
                  
                    allActiveImages.Add(newImage);
                    allActiveTexts.Add(textBlock);
                    undo.Add(new Tuple<string, Image>("remove", newImage));
                    undoText.Add(new Tuple<string, TextBlock>("remove", textBlock));

                    redo.RemoveRange(0, redo.Count);
                    redoText.RemoveRange(0, redoText.Count);

                    textSize = 10;

                    pairs.Add(newImage, textBlock);

                    newImage = null;
                    newTextBlock = null;
                }
               

            }
        }
        private List<Shape> Deleted = new List<Shape>();
        private bool sakriveno = false;
        private void btnSakriNeaktivno_Click(object sender, RoutedEventArgs e)
        {
            if (sakriveno)
            {
                foreach (var item in Deleted)
                {
                    try
                    {
                        drawing_canvas.Children.Add(item);
                    }
                    catch
                    {

                    }
                }
                Deleted.Clear();
                sakriveno = false;
            }
            else
            {
                List<Shape> switches = new List<Shape>();
                foreach (Shape element in drawing_canvas.Children)
                {

                    if (element.ToolTip != null && element.ToolTip.ToString().Contains("Switch") && element.ToolTip.ToString().Contains("Status: Open"))
                    {
                        switches.Add(element);
                    }
                }
                HashSet<long> ids = FindIds(switches);
                foreach (Shape element in drawing_canvas.Children)
                {
                    if (element.ToolTip != null && element.ToolTip.ToString().Contains("Power line"))
                    {
                        long id = long.Parse(element.ToolTip.ToString().Split(' ', '\n')[3]);
                        LineEntity lineEntity = db.lineEntities.FirstOrDefault(x => x.Id == id);
                        if (ids.Contains(lineEntity.FirstEnd))
                        {
                            Deleted.Add(element);
                            DeleteSecondEnd(lineEntity.SecondEnd);
                        }
                    }
                }
                foreach (var item in Deleted)
                {
                    drawing_canvas.Children.Remove(item);
                }
                sakriveno = true;
            }

        }

        private void DeleteSecondEnd(long secondEnd)
        {
            foreach (Shape element in drawing_canvas.Children)
            {
                if (element.ToolTip != null && element.ToolTip.ToString().Contains(secondEnd.ToString()))
                { 
                    Deleted.Add(element);
                    return;
                }
            }
        }

        private HashSet<long> FindIds(List<Shape> switches)
        {
            HashSet<long> ids = new HashSet<long>();
            foreach (var item in switches)
            {
                long result = 0;
                long.TryParse(item.ToolTip.ToString().Split(' ', '\n')[1],out result);
                if(result!=0)
                {
                    ids.Add(result);
                }
            }
            return ids;
        }
         

        private void cmbOboj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbOboj.SelectedIndex)
            {
                case 0:
                    ObojPocetno();
                    break; 
                case 1:
                    ObojPoMaterijalu();
                    break;
                case 2:
                    ObojPoOtpornosti();
                    break; 
            }
        }

        private void ObojPoMaterijalu()
        {
            foreach (Shape element in drawing_canvas.Children)
            {
                if (element.ToolTip != null && element.ToolTip.ToString().Contains("Power line"))
                {

                    long id = long.Parse(element.ToolTip.ToString().Split(' ', '\n')[3]);
                    LineEntity lineEntity = db.lineEntities.FirstOrDefault(x => x.Id == id); 
                    if(lineEntity.ConductorMaterial=="Steel")
                    {
                        element.Stroke = new SolidColorBrush(Colors.Gray);
                        element.StrokeThickness = 1;
                    }
                    else if(lineEntity.ConductorMaterial=="Acsr")
                    {
                        element.Stroke = new SolidColorBrush(Colors.DimGray);
                        element.StrokeThickness = 1;
                    }
                    else if (lineEntity.ConductorMaterial== "Copper")
                    {
                        element.Stroke = new SolidColorBrush(Colors.Orange);
                        element.StrokeThickness = 1;
                    }
                    else if (lineEntity.ConductorMaterial== "Other")
                    {
                        element.Stroke = new SolidColorBrush(Colors.Black);
                        element.StrokeThickness = 1;
                    } 
                }
            }
        }

        private void ObojPoOtpornosti()
        {
            foreach (Shape element in drawing_canvas.Children)
            {
                if (element.ToolTip != null && element.ToolTip.ToString().Contains("Power line"))
                {
                    long id = long.Parse(element.ToolTip.ToString().Split(' ', '\n')[3]);
                    LineEntity lineEntity = db.lineEntities.FirstOrDefault(x => x.Id == id);
                    if (lineEntity.R<1)
                    {
                        element.Stroke = new SolidColorBrush(Colors.Red); 
                    }
                    else if (lineEntity.R<2)
                    {
                        element.Stroke = new SolidColorBrush(Colors.Orange); 
                    }
                    else
                    {
                        element.Stroke = new SolidColorBrush(Colors.Yellow); 
                    }
                    element.StrokeThickness = 1;
                }
            }
        }
         

        private void ObojPocetno()
        {
            foreach (Shape element in drawing_canvas.Children)
            {
                if (element.ToolTip != null && element.ToolTip.ToString().Contains("Power line"))
                {
                    element.Stroke = new SolidColorBrush(Colors.Black);
                    element.StrokeThickness = 1;
                }
            }
        }

        private void bntPromenaBoje_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush switches=new SolidColorBrush(), substations = new SolidColorBrush(), nodes = new SolidColorBrush(); 
            SelectColorWindow selectColorWindow = new SelectColorWindow(switches,substations,nodes);
            selectColorWindow.ShowDialog();
            foreach (Shape element in drawing_canvas.Children)
            {
                if (element.ToolTip != null && element.ToolTip.ToString().Contains("Substation") && substations != new SolidColorBrush())
                {
                    element.Fill = substations;
                    substationEllipse.Fill = substations;
                }
                if (element.ToolTip != null && element.ToolTip.ToString().Contains("Switch") && switches!=new SolidColorBrush())
                {
                    element.Fill = switches;
                    switchEllipse.Fill = switches;
                }
                if (element.ToolTip != null && element.ToolTip.ToString().Contains("Node") && nodes != new SolidColorBrush())
                {
                    element.Fill = nodes;
                    nodeEllipse.Fill = nodes;
                }
            }
        }

        private void btnPoKonektivnosti_Click(object sender, RoutedEventArgs e)
        {
            foreach (Shape element in drawing_canvas.Children)
            {
                if (element.ToolTip != null && (element.ToolTip.ToString().Contains("Substation") || element.ToolTip.ToString().Contains("Switch") || element.ToolTip.ToString().Contains("Node")))
                {
                    long id = long.Parse(element.ToolTip.ToString().Split(':', '\n')[1]);
                    PowerEntity powerEntity = db.FindElement(id);
                    if(powerEntity.Connections<3)
                    {
                        element.Fill = new SolidColorBrush(Color.FromRgb(255, 204, 204));
                    }
                    else if(powerEntity.Connections<5)
                    {
                        element.Fill = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        element.Fill = new SolidColorBrush(Color.FromRgb(128, 0,0));
                    }

                } 
            }
        }
        private void save_FrameworkElement_as_Screenshot_File(FrameworkElement element)
        {
            String filename = "sceenshot-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)element.ActualWidth, (int)element.ActualHeight, 96, 96, PixelFormats.Pbgra32);

            bmp.Render(element);
            PngBitmapEncoder encoder = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(bmp));

            FileStream fs = new FileStream(filename, FileMode.Create);

            encoder.Save(fs);

            fs.Close();
            MessageBox.Show("Uspesno screenshotovano");

        }

        private void btnScreenshot_Click(object sender, RoutedEventArgs e)
        {
            save_FrameworkElement_as_Screenshot_File(drawing_canvas);
        }

        //From UTM to Latitude and longitude in decimal
        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
		{
			bool isNorthHemisphere = true;

			var diflat = -0.00066286966871111111111111111111111111;
			var diflon = -0.0003868060578;

			var zone = zoneUTM;
			var c_sa = 6378137.000000;
			var c_sb = 6356752.314245;
			var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
			var e2cuadrada = Math.Pow(e2, 2);
			var c = Math.Pow(c_sa, 2) / c_sb;
			var x = utmX - 500000;
			var y = isNorthHemisphere ? utmY : utmY - 10000000;

			var s = ((zone * 6.0) - 183.0);
			var lat = y / (c_sa * 0.9996);
			var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
			var a = x / v;
			var a1 = Math.Sin(2 * lat);
			var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
			var j2 = lat + (a1 / 2.0);
			var j4 = ((3 * j2) + a2) / 4.0;
			var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
			var alfa = (3.0 / 4.0) * e2cuadrada;
			var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
			var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
			var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
			var b = (y - bm) / v;
			var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
			var eps = a * (1 - (epsi / 3.0));
			var nab = (b * (1 - epsi)) + lat;
			var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
			var delt = Math.Atan(senoheps / (Math.Cos(nab)));
			var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

			longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
			latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
		}
	}
}
