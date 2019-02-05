using CAVS.ProjectOrganizer.Project;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{

    public class CarManager
    {

        private static CarManager instance;

        public static CarManager Instance()
        {
            if (instance == null)
            {
                instance = new CarManager();
            }
            return instance;
        }

        public delegate void GarageChangeEvent(PictureItem[] cars);

        public event GarageChangeEvent OnGarageChange;

        public delegate void MainCarChangeEvent(PictureItem car);

        public event MainCarChangeEvent OnMainCarChange;

        /// <summary>
        /// All the cars we're going to display information about. 
        /// </summary>
        private PictureItem[] cars;

        /// <summary>
        /// The car everyone should be displaying the most information about
        /// </summary>
        private PictureItem mainCar;

        /// <summary>
        /// Made private for singleton usage
        /// </summary>
        private CarManager()
        {
            cars = new PictureItem[0];
        }

        public void SetGarage(PictureItem[] carsToDisplay)
        {
            cars = carsToDisplay;
            OnGarageChange?.Invoke(cars);
            SetMainCar(null);
        }

        public PictureItem[] Garage()
        {
            return cars;
        }

        public void SetMainCar(PictureItem mainCar)
        {
            this.mainCar = mainCar;
            OnMainCarChange?.Invoke(mainCar);
        }

        public PictureItem GetMainCar()
        {
            return mainCar;
        }

        public int GarageSize()
        {
            return cars.Length;
        }

    }

}