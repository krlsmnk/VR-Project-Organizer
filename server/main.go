package main

import (
	"os"

	prosign "gitlab.com/prosign/server"
)

type personalLogger struct {
}

func (logger personalLogger) Log(log string) {

}

func main() {
	roomService := prosign.NewRoomService(&personalLogger{})
	roomService.InterceptIncomingUpdate(func(message string, guest prosign.Guest) (string, error) {
		return message + guest.ID(), nil
	})
	server := prosign.NewEmptyServer(os.Getenv("PROSIGN_DEBUG") == "true")
	server.RegisterService("room", roomService)
	server.Listen(":3000")
}
