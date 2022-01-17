using Helpers;
using MvvmHelpers;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lines.ViewModels
{
    //I'm using A star pathfinding algorithm to find path between two selected points and for omit obstackles


    public class LinesViewModel : BaseViewModel
    {
        public LinesViewModel()
        {
            Vectors = new ObservableCollection<Vector>();
            MouseDownCommand = new RelayCommand(async t => await OnClick(t));
            BoardHeight = 400;
            BoardWidth = 800;
            Scale = 4;
        }

        int boardWidth;
        public int BoardWidth
        {
            get { return boardWidth; }
            set { SetProperty(ref boardWidth, value); }
        }

        int boardHeight;
        public int BoardHeight
        {
            get { return boardHeight; }
            set { SetProperty(ref boardHeight, value); }
        }

        //scale is used for make less calculations
        //many points on board can be described as one big point

        int scale;
        public int Scale
        {
            get { return scale; }
            set { SetProperty(ref scale, value);}
        }
        public ObservableCollection<Vector> Vectors { get; set; }
        public Point TrackedPoint { get; set; }
        public Point StartPoint { get; set; }

        public List<Point> DisabledPoints { get; set; } = new List<Point>();
        public List<Point> CheckedPoints { get; set; }

        string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }


        void Restart()
        {
            this.StartPoint = null;
            this.TrackedPoint = null;
        }

        public ICommand MouseDownCommand { get;set; } 
        async Task OnClick(object parameter)
        {
            ErrorMessage = null;
            if (parameter as Point == null)
                throw new ArgumentException();
            TrackedPoint = parameter as Point;
            if(StartPoint == null)
            {
                StartPoint = parameter as Point;
                ErrorMessage = null;
            }
            else
            {
                if(StartPoint.X == TrackedPoint.X && StartPoint.Y == TrackedPoint.Y)
                {
                    this.ErrorMessage = "You chosed the same points";
                    Restart();
                    return;
                }
                StartPoint.ScalePosition(Scale);
                TrackedPoint.ScalePosition(Scale);
                
                var points = await AStarAlgorithm.GetPath(StartPoint, TrackedPoint, DisabledPoints, Scale, BoardWidth, BoardHeight);
                if(points == null)
                {
                    ErrorMessage = "There's no path between selected points";
                    Restart();
                    return;
                }

                for(int i =0;i < points.Count-1; i++)
                {
                    Vectors.Add(new Vector(points[i], points[i + 1]));
                    DisabledPoints.Add(points[i]);
                    if(points[i].X != points[i+1].X && points[i].Y != points[i + 1].Y) // set additional Obstackles for oblique points
                    {
                        DisabledPoints.Add(new Point( points[i].X , points[i + 1].Y));
                        DisabledPoints.Add(new Point(points[i+1].X, points[i].Y));
                    }
                }
                Restart();
            }
        }

       
        //void AddPath(Point _startPoint, Point _endPoint)
        //{
        //    Vector newVector = new Vector(_startPoint, _endPoint);
        //    try
        //    {
        //        double aParameter = LinearFunctionHelper.GetAParameter(_startPoint.X, _startPoint.Y, _endPoint.X, _endPoint.Y);
        //        double bParameter = LinearFunctionHelper.GetBParameter(_startPoint.X, _startPoint.Y, aParameter);
        //        newVector.LinearDescription = new LinearFunction(aParameter, bParameter);
        //    }
        //    catch(Line0YParallelexception ex)
        //    {
        //        newVector.LinearDescription = new LinearFunction(_startPoint.X);
        //    }
        //    catch(ArgumentException ex)
        //    {
        //        ErrorMessage = ex.Message;
        //        StartPoint = null;
        //        return;
        //    }
        //    if(Vectors.Any(d=> VectorHelper.AreVectorsCrossing(d, newVector)))
        //    {
        //        StartPoint = null;
        //    }
        //    else
        //    {
        //        Vectors.Add(newVector);
        //        StartPoint = null;
        //    }
        //}

        //Vector CreateNewVector(Point aPoint, Point bPoint)
        //{

            
        //    Vector newVector = new Vector(aPoint, bPoint);
        //    try
        //    {
        //        double aParameter = LinearFunctionHelper.GetAParameter(aPoint.X, aPoint.Y, bPoint.X, bPoint.Y);
        //        double bParameter = LinearFunctionHelper.GetBParameter(aPoint.X, aPoint.Y, aParameter);
        //        newVector.LinearDescription = new LinearFunction(aParameter, bParameter);
        //    }
        //    catch (Line0YParallelexception ex)
        //    {
        //        newVector.LinearDescription = new LinearFunction(aPoint.X);
        //    }

        //    return newVector;
        //}

        //void AddPath2(Point _startPoint, Point _targetPoint)
        //{
            
        //    var newVector = CreateNewVector(_startPoint, _targetPoint);

        //    var blockingVectors = Vectors.Where(d => VectorHelper.AreVectorsCrossing(d, newVector)).ToList();
        //    if(blockingVectors.Count() > 0)
        //    {
        //        var nearestPointToTarget = blockingVectors.Select(d => d.APoint).Concat(blockingVectors.Select(d => d.BPoint)).OrderBy(d => d.GetDistance(_targetPoint)).ToList().First();
        //        var vectorToOmit = blockingVectors.Where(d=> d.APoint.Equals(nearestPointToTarget) || d.BPoint.Equals(nearestPointToTarget)).Single();


        //        var vect = CreateNewVector(_startPoint, nearestPointToTarget);
        //        int deltaX = _startPoint.X > nearestPointToTarget.X ? 3 : -3;
        //        int deltaY = _startPoint.Y > nearestPointToTarget.Y ? 3 : -3;

        //        nearestPointToTarget.X += deltaX;
        //        //nearestPointToTarget.Y += deltaY * vectorToOmit.LinearDescription.AParameter;



        //        Vectors.Add(vect);
        //        AddPath2(nearestPointToTarget, _targetPoint);
                
        //    }
        //    else
        //    {
        //        Vectors.Add(newVector);
        //    }
        //    StartPoint = null;
            
        //    //if (Vectors.Any(d => VectorHelper.AreVectorsCrossing(d, newVector)))
        //    //{
        //    //    StartPoint = null;
        //    //}
        //    //else
        //    //{
        //    //    Vectors.Add(newVector);
        //    //    StartPoint = null;
        //    //}
        //}
    }


    //typical implementation of ICommand
    public class RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }


}
