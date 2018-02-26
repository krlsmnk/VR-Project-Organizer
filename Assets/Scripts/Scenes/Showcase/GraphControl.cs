using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CAVS.ProjectOrganizer.Project.Aggregations.Plot;
using CAVS.ProjectOrganizer.Project;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{

    /// <summary>
    /// Meant managing the entire carshowcase scene.
    /// </summary>
    public class GraphControl : MonoBehaviour
    {

        /// <summary>
        /// The manager of the scene
        /// </summary>
        private SceneManagerBehavior sceneManager;

        private Item[] items;

        private string[] columnsToExamine;

        private int x;

        private int y;

        private int z;

        private GameObject oldPlot;

        [SerializeField]
        private InteratibleButtonBehavior xButtonToggle;

        [SerializeField]
        private InteratibleButtonBehavior yButtonToggle;

        [SerializeField]
        private InteratibleButtonBehavior zButtonToggle;

        [SerializeField]
        private Text xText;

        [SerializeField]
        private Text yText;

        [SerializeField]
        private Text zText;

        public void Initialize(SceneManagerBehavior sceneManager, Item[] items)
        {

            this.sceneManager = sceneManager;
            this.items = items;

            x = 0;
            y = 1;
            z = 2;

            columnsToExamine = new string[]{
                "Year",
                "Length (in)",
                "Width (in)",
                "Height (in)",
                "Wheelbase (in)",
                "Curb weight (lbs)",
                "Horsepower (HP)"
            };

            xButtonToggle.Subscribe(delegate ()
            {
                x++;
                if (x >= columnsToExamine.Length)
                {
                    x = 0;
                }
                Render();
            });

            yButtonToggle.Subscribe(delegate ()
            {
                y++;
                if (y >= columnsToExamine.Length)
                {
                    y = 0;
                }
                Render();
            });

            zButtonToggle.Subscribe(delegate ()
            {
                z++;
                if (z >= columnsToExamine.Length)
                {
                    z = 0;
                }
                Render();
            });

            Render();
        }

        private void Render()
        {
            if (oldPlot != null)
            {
                Destroy(oldPlot);
            }

            Debug.Log(new Vector3(x, y, z));

            GameObject plot = new ItemPlot(items, columnsToExamine[x], columnsToExamine[y], columnsToExamine[z])
                .Build(Vector3.one * 4, sceneManager.PlotPointBuilder);

            plot.transform.position = new Vector3(-1.5f, 1, -5);
            plot.transform.LookAt(new Vector3(0, 1, 0));

            xText.text = "X: " + columnsToExamine[x];
            yText.text = "Y: " + columnsToExamine[y];
            zText.text = "Z: " + columnsToExamine[z];

            oldPlot = plot;
        }

    }

}