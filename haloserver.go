package main

import (
	"fmt"
	"net/http"
	"strconv"
)

// Vehicle represents the current vehicle in the showcase
type Vehicle int

func (v Vehicle) String() string {
	return fmt.Sprintf("v %d\n", v)
}

// Position represents a position in 3D space
type Position [3]float64

// NewPosition returns a new position
func NewPosition(x, y, z float64) Position {
	return [3]float64{x, y, z}
}

func (p Position) String() string {
	return fmt.Sprintf("x %f\ny %f\nz %f\n", p[0], p[1], p[2])
}

func main() {
	var vehicle Vehicle = 2

	position := NewPosition(0, 0, 0)

	http.HandleFunc("/", func(w http.ResponseWriter, r *http.Request) {
		fmt.Fprint(w, position.String()+vehicle.String())
	})

	http.HandleFunc("/set", func(w http.ResponseWriter, r *http.Request) {

		var xParsed float64
		var yParsed float64
		var zParsed float64
		var err error

		x := r.URL.Query().Get("x")
		if x != "" {
			xParsed, err = strconv.ParseFloat(x, 64)
			if err != nil {
				fmt.Fprintf(w, "Error: parsing x value: %s", err.Error())
				return
			}
		} else {
			fmt.Fprintf(w, "Error: Please provide an x value")
			return
		}

		y := r.URL.Query().Get("y")
		if y != "" {
			yParsed, err = strconv.ParseFloat(y, 64)
			if err != nil {
				fmt.Fprintf(w, "Error: parsing y value: %s", err.Error())
				return
			}
		} else {
			fmt.Fprint(w, "Error: Please provide an y value")
			return
		}

		z := r.URL.Query().Get("z")
		if z != "" {
			zParsed, err = strconv.ParseFloat(z, 64)
			if err != nil {
				fmt.Fprintf(w, "Error: parsing z value: %s", err.Error())
				return
			}
		} else {
			fmt.Fprint(w, "Error: Please provide an z value")
			return
		}

		position = NewPosition(xParsed, yParsed, zParsed)

		v := r.URL.Query().Get("v")
		if v != "" {
			vParsed, err := strconv.ParseInt(v, 10, 64)
			if err != nil {
				fmt.Fprintf(w, "Error: parsing v value: %s", err.Error())
				return
			}
			vehicle = Vehicle(int(vParsed))
		} else {
			fmt.Fprint(w, "Error: Please provide an v value")
			return
		}

		fmt.Fprint(w, "values set")

	})

	http.ListenAndServe(":2019", nil)
}
