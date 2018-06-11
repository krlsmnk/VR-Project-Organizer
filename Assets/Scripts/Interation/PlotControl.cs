using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CAVS.ProjectOrganizer.Project.Aggregations.Plot;
using CAVS.ProjectOrganizer.Project;

namespace CAVS.ProjectOrganizer.Interation
{

    /// <summary>
    /// Meant managing the entire carshowcase scene.
    /// </summary>
    public class PlotControl : MonoBehaviour
    {

        private Func<Item, GameObject> builder;

        private Action<GameObject> newPlotCallback;

        private Item[] items;

        private string[] columnsToExamine;

        private int x;

        private int y;

        private int z;

        private GameObject oldPlot;

        [SerializeField]
        private ButtonBehavior xButtonToggle;

        [SerializeField]
        private ButtonBehavior yButtonToggle;

        [SerializeField]
        private ButtonBehavior zButtonToggle;

        [SerializeField]
        private Text xText;

        [SerializeField]
        private Text yText;

        [SerializeField]
        private Text zText;

        public void Initialize(Action<GameObject> newPlotCallback, Item[] items)
        {
            this.newPlotCallback = newPlotCallback;
            Initialize(null, items);
        }

        public void Initialize(Func<Item, GameObject> builder, Item[] items)
        {

            this.builder = builder;
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

            GameObject plot;

            if (builder != null)
            {
                plot = new ItemPlot(items, columnsToExamine[x], columnsToExamine[y], columnsToExamine[z])
                    .Build(Vector3.one * 4, builder);
            }
            else
            {
                plot = new ItemPlot(items, columnsToExamine[x], columnsToExamine[y], columnsToExamine[z])
                    .Build(Vector3.one * 4);
            }

            plot.transform.position = new Vector3(-1.5f, 1, -5);
            plot.transform.LookAt(new Vector3(0, 1, 0));

            xText.text = "X: " + columnsToExamine[x];
            yText.text = "Y: " + columnsToExamine[y];
            zText.text = "Z: " + columnsToExamine[z];

            oldPlot = plot;
            if (newPlotCallback != null)
            {
                newPlotCallback(oldPlot);
            }
        }

    }

}