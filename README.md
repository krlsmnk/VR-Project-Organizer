# Dear whoever is taking over the project

I will try to give an understanding of the innerworkings of this project, and explain the reasoning behind the design decisions made. Before I get into that, you will need to bring yourself up to speed on some very crucial topics of Object Orriented Programming, how C# works, and other Software Engineering Topics to have a full understanding of the codebase. I will list out some of these topics and their importance, and then it is up to you to educate yourself.

Topics to understand for the project:
1. [Abstract Classes](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/abstract-and-sealed-classes-and-class-members)
2. [Interfaces](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface)
3. [Static keyword](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static) and why [you should use them rarely](https://simpleprogrammer.com/static-methods-will-shock-you/).
4. Why you [never make variables public](https://softwareengineering.stackexchange.com/questions/143736/why-do-we-need-private-variables), and C#'s [property](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties) alternative
5. Git, and good git practices.
    * How to merge branches and resolve conflicts (Extremely important for Unity Scenes)
6. Unity's Vector3 operator overloads (allows you to do a lot with few lines of code)
7. Unity supports [Quaternion * Vector3](https://docs.unity3d.com/ScriptReference/Quaternion-operator_multiply.html). This will save you in obscure situations.
8. [Quaternions](https://www.youtube.com/watch?v=mHVwd8gYLnI) 
9. [Design Patterns](https://sourcemaking.com/design_patterns)
    * [Singletons](https://twitter.com/jasperstocker/status/1105478975588106242/photo/1)
    * Factory
    * Momentos
10. Anonymous functions (delegates) like [Func](https://docs.microsoft.com/en-us/dotnet/api/system.func-1?view=netframework-4.7.2) and [Action](https://docs.microsoft.com/en-us/dotnet/api/system.action-1?view=netframework-4.7.2).
11. C#'s [event](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/event) keyword. (VRTK uses it heavily, I use it in some places) 
12. C# Naming/Styling conventions.
13. C# [Namespaces](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/namespaces/) 
14. LEARN THE REFACTOR TOOLS OF VISUAL STUDIOS. LET THE EDITOR WRITE THE CODE FOR YOU
15. string.Format()
16. Avoid booleans for variables. Look into [enum](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/enum)s
17. If you're going to be working with networking, look into [Docker](https://www.docker.com/). This is an amazing tool in itself that most tech companies have adopted in some form or fashion. It's worth learning if you don't know it yet.
18. If you're working with UI-Gen project, or just want to get better at Unity, look into [Asset Bundles](https://docs.unity3d.com/Manual/AssetBundlesIntro.html). Resources.Load is honestly a horrible way of doing things but I kept it that way for sake of simplicity. Developing real games will require the use of asset bundles and careful planning on how you're going to use them, which is game specific.

Personal conventions on project:

1. Any code goes in the Script folder.
2. Create a folder for scripts per scene.
3. Namespaces follow folder structure once inside Scripts folder, and are prepended with CAVS.ProjectOrganizer.
    * For example, if a script is located under Scripts/Interaction, the namespace would then be CAVS.ProjectOrganizer.Interaction.
4. Any class that inherits from MonoBehavior has their name appended with "Behavior".
    * For example, If I see a class named GameManagerBehavior, I know it inherits from monobehavior and if I have an instance of it, it must exist in the scene.
    * If the class is just named GameManager, then I know it does not inherit from monobehavior, and it's existance is not tied to the scene loaded.
5. Each Scene will have their own GameManagerBehavior class that is attatched to a empty object in the scene named "SCRIPTS\".
6. Avoid the use of tags as much as possible.

RULES TO HOLD YOURSELF TO:
1. Don't change existing libraries code unless absilutely necessary. IE: Don't change code under the VRTK folder. It's almost always possible to extend functionality in some way through options like inheritance. In the entire lifetime of the project I have had to make changes once.
2. Be very very very very very very very thoughtful with your variable, function, and class naming. This proper naming servese as the best form of documentation.

## How To Build

Building and exporting the game goes like any normal unity project. However the showcase relies on data from a csv for the car data. Assuming you built the project under a folder `ProjectOrganizer`, you will need to place the csv data under:

```
ProjectOrganizer\ProjectOrganizer_Data\Resources
```

Upon launching the application you should have the cares appear. If they do not then you probably placed the csv in the wrong location.

## Some Tips

### Constructors for Monobehavior

Unity prevents classes who inherit from monobehaior from having their own constructors. This makes sense, as those types of scripts must be attatched to game objects. However it is annoying not having a nice function call that will automatically create one, while allowing you to set it's variables simultaniously. To get around this I have created a static method on certain MonoBehavior classes in which I desire that functionality called Initialize(). Some of these functions take extra parameters specific to the class. For example, this is what part of GrabBehaviorLooks like:

```C#
using UnityEngine;
using VRTK;
using CAVS.ProjectOrganizer.Interation;

namespace CAVS.ProjectOrganizer.Controls
{

    public class GrabControlBehavior : MonoBehaviour
    {

        public static GrabControlBehavior Initialize(VRTK_ControllerEvents hand)
        {
            var newScript = hand.gameObject.AddComponent<GrabControlBehavior>();

            newScript.hand = hand;
            newScript.hand.GripPressed += newScript.Hand_GripPressed;
            newScript.hand.TriggerClicked += newScript.Hand_TriggerPressed;
            newScript.hand.TriggerUnclicked += newScript.Hand_TriggerReleased;
            newScript.hand.TouchpadAxisChanged += newScript.TouchpadAxisChanged;
            newScript.hand.TouchpadTouchEnd += newScript.Hand_TouchpadTouchEnd;

            return newScript;
        }

    }

}
```

Now to attach this behavior to a hand, instead of having ot call Addcomponent and make seperate calls to set the variables, all I have to do is say.

```C#
GrabControlBehavior.Initialize(hand);
```

And the behavior attaches itself!

### Not Bothering With Comments

So what is this project? I think it's best explained in this graph:

![Graph](https://i.imgur.com/nUdDtOp.png)

Our Goal is not clear, nor is our solution. So we have ourselves an [extreme project](https://en.wikipedia.org/wiki/Extreme_project_management) which means I've been having to do [extreme programming](https://en.wikipedia.org/wiki/Extreme_programming) (a poor job of it at that). If we had frequent communication with the client this project would have resembled more of an extreme one, but honestly the whole thing has felt like how [southpark determines property value](https://youtu.be/xGTIKTH28BY?t=368).

Anyways what does this have to do with comments? With extreme programming and this project comes accepting that whatever you do can/will be discarded. Almost the entirity of the code under Scripts/Project (a lot) is no longer being used. Nor is any of the networking code I wrote is being used. Spending time commenting that code would have taken forever, and ultimately a waste of time.

More than just being a waste of time, there are very good arguments as to why you don't want to overly comment your code. It can be summarised as:

> Good Code is Self Documenting

Focus writing small functions with good names. Look at this *ugly* code: 

```C#
private IEnumerator LoadImageAsync(string url)
{
    System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(url);
    byte[] hash = md5.ComputeHash(inputBytes);

    string sb = "";
    for (int i = 0; i < hash.Length; i++)
    {
        sb += hash[i].ToString("X2");
    }

    string hashName = sb.ToString();
    string[] chunks = url.Split('.');
    string extension = chunks[chunks.Length - 1];
    string filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "cache"), hashName + "." + extension);

    // other stuff...
}
```

There's a lot going on to load this image. More importantly it's doing multiple jobs to accomplish it's goal, meaning the function can logically be broken up.

```C#
private IEnumerator LoadImageAsync(string url)
{
    string filPathForSaving = DetermineCachedFilePathForSavingImage(url)

    // other stuff...
}

/// <summary>
/// Given a url, determine the filepath on the system that it's contents can be saved too.
/// </summary>
/// <param name="input">Url</param>
/// <returns>Filepath location</returns>
private static string DetermineCachedFilePathForSavingImage(string url) 
{
    string hashName = CalculateHash(url);
    string[] chunks = url.Split('.');
    string extension = chunks[chunks.Length - 1];
    return Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "cache"), hashName + "." + extension);
}

/// <summary>
/// Calculates the MD5 Hash of a string
/// </summary>
/// <param name="input">the string to calculate an MD5 hash from</param>
/// <returns>Md% hash</returns>
private static string CalculateHash(string input)
{
    System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
    byte[] hash = md5.ComputeHash(inputBytes);

    string sb = "";
    for (int i = 0; i < hash.Length; i++)
    {
        sb += hash[i].ToString("X2");
    }

    return sb.ToString();
}
```
These functions work as black boxes. You don't really have to care about how the hash is being calculated, you just need to know that the function does that. And guess how you know the function does that? By it's name. Making any comments like I've done on it is fucking redundant and wastes time. Also this example is *code that is no longer being used in the project.*

And probably the biggest reason not to leave comments in an extreme project: things change. I can change a function name by hitting f2 and it'll be renamed throughout the entire project, keeping everything up to date. You know what won't change? The comment I left explaining it and what it does. The compiler doesn't care about my comments. It won't tell me when my comment is out of date, that I'm telling another developer a lie. The comment will just lay their unchanged cause that function edit was one of 20 I am going to be making that single commit. That commit is going to be 1 of 100s in the project. You know what will happen though? Another developer is going to come along and instead of reading the code itself, they will take the comment at face value and make assumptions that are no longer true. What comes next is a nice long debug session for something that could have been avoided if better naming of methods where employed.

There are still scenarios in which I comment my code. Unfortunatly, comments are sometimes necessary. They are especially important if you're building libraries for hundres/thousands of other developers to use. Libraries that have clear goals. Just, take away from this section:

> Refactor your comments into better code

/rant

# Project Organization

This project has been broken up into smaller projects for doing very specific things. Much of the intracies of ANVEL have been encapsulated into it's own unity project. Prosign (The server code) is spread across 3 repositories. The UI Gen also has it's own repository. 

Here are their repositories respectfully:
1. [Prosign](https://gitlab.com/prosign)
2. [UI Gen](https://github.com/EliCDavis/UI-Gen)
3. [Anvel](https://github.com/EliCDavis/Anvel-To-Unity)
4. [This project](https://github.com/EliCDavis/VR-Project-Organizer)

Generally what you see in these smaller libraries' projects is that I've put everything under Assets into a subfolder called the name of the Project. So all code for Prosign is under Assets/Prosign/Scripts instead of Assets/Scripts. The reason for doing is for when I need to export this project as a unity package. Doing it this way makes it unpack into the main project as Assets/Prosign, instead of placing all the scripts under the main scripts folder. This keeps things nice and seperated.

## Anvel

Fundamental abstractions between Unity and Anvel exist in this repository. It is meant for dealing with basics such as establishing a connection with Anvel and managing objects in the Anvel world. When I first started coding this I made the assumption I should be trying to reuse the same connection as much as possible, so some functionality requires a AnvelControlService.Client object to work. However as I introduced multithreading into the project this kind of mindset actually made things worse not better. The reason is the multiple threads all try writing multple things to the same socket, causing very weird bugs that where un reporducable by nature. Once I realized that issue, future additions to the code base instead accepted ClientConnectionToken objects instead. These tokens keep up with the ip address and port anvel is running on that it needs to connect too. So if I wanted to create a lidar display, I would need to pass it a connection token instead a connection itself. The Lidar Display would then create it's own connection that it would use for recieving lidar data in a seperate thread. Having things set up this way is also advantageous because we can easily have one lidar sensor pull from one instance from anvel, and another lidar sensor pull from another seperate instance.

I have created a wrapper for Anvel's ObjectDescriptor called AnvelObject. You can crate an AnvelObject from a ObjectDescriptor and it will allow you to do more Unity like things. For example, if you need to rotate the object inside of anvel, you can use Unity's quaternions and it will do the conversion for you. Creating instances of AnvelObject allows you to not have to worry about cleanup in the Anvel scene. When unity stops my code goes through
Anvel and deletes everything that we have created.

Something important I've learned about anvel is that one anvel "object" can have multiple "objects" attatched to it. For example even if you create a lidar sensor, you won't have access to the lidar sensor properties. You will have to then search for the properties. Example code below:

```C#
var name = "myLidar1234";

// Create a Lidar object in anvel, which gives you an ObjectDescriptor for doing anvel things
// Setting lidar specific properties such as range will throw errors
var baseObj = connection.CreateObject(
    AssetName.Sensors.API_3D_LIDAR, 
    name, 
    parent.ObjectDescriptor().ObjectKey, 
    new Point3(), 
    new Euler(), 
    false);

// You have to re find the object you have created as a lidar type now. You
// will then be able to set lidar specific properties
var actualLidarSensor = connection.GetObjectDescriptorByTypeAndName("APILidar", name)
```

The name "APILidar" cooresponds to the grouping of variables you see inside the anvel window. So if you where dealing with other variables you would look towards that group's name to get a referennce.

![anvelinfo](https://i.imgur.com/9emv3HT.png)

Here's just a bunch of quicktips:

1. Wrap anvel ObjectDesctiptors in AnvelObject to ensure cleanup
2. When indoubt, look at their [Api Reference](https://wiki.anvelsim.com/3/index.php/EXTERNALAPI)
3. If you're doing stuff like pulling sensor information, create multiple connections instead of reusing
4. Anvel will error out if you try to create an object with a name that is already being used by another object.

If Anvel becomes more and more havily used. You will want to invest into [object pooling](https://sourcemaking.com/design_patterns/object_pool) for reusing connections. 

## Prosign

The entirety of the server side code for Prosign was written in the programming language Golang and deployed using the containerization software called Docker. A test-driven approach was adopted early on to ensure new code coming into the project does not break existing functionality. Golang's [mockgen tool](https://github.com/golang/mock) was used to automatically generate mock structs that implement interfaces defined in the code to write more sophisticated unit tests. Some of these more involved tests attempt to ensure the server responds appropriately to failure at different points in execution. Perhaps the most useful tests in the development of the server were the integration tests. In these tests, both the server and two clients were spawned and one client would attempt to talk to the other by sending and receiving messages from the server. To write these tests, the clients would have to implement the protocol to properly communicate with the server. Once these tests successfully passed not only had the server been tested from end to end, but now there exists an example in Golang of how to communicate with the server for reference. Gitlab's Continuous Integration server was used to automatically build and run all the tests that existed with the code to ensure nothing was broken, as well as compute test coverage to ensure only code that has been tested is being committed. At the time of this writing, 81.5% of the server's code is covered by tests. 

![Architecture](https://i.imgur.com/HBmBMWV.png)

An instance of a running Prosign server is made up of a collection of services and guests.  All services used by the server are registered at startup. Throughout the lifetime of the server, the services will receive messages from multiple guests. A guest is something that can both send and receive messages, as well as be notified of errors. A guest was originally just a TCP socket but has since been abstracted away into its own interface. In doing so services can communicate with many different types of guests with no extra work. If the server was ever required to be able to communicate with a web page through web-sockets, the only code that would need to be added is strictly web-socket code that implements the guest interface. Once completed, two-way communication between a web page and a TCP socketed application like Unity3D would be possible. Implementing the guest interface to account for executables that were launched by the application would be trivial due to the fact Golang's executables and sockets share the same read and write interfaces.

### Protocol

Incoming messages from a guest are arranged into three parts. The first part is the message size which is represented as a single byte. The second part is the header which contains two components: the service for the message and the functionality requested from the service known as the method. The third part of the message is the message body which is the remaining payload for any additional information. The message body is optional depending on the service and method in question. A service is not required to send a response back to the client.

![message structure](https://i.imgur.com/KeeHXgO.png)

Messages sent from the server are constructed almost exactly the same way as the client. The one difference is that one extra byte is added at the very beginning of the message to denote status. A status of 0 means the message is a normal message, and a status of 1 means the message is an error message stating something illegal was attempted. An error message is always a response to a message the client had sent previously and can be formulated by either the service or the server itself. The body of the message is the description of the error.

### Services

The first default service in Prosign is called Hotel. Hotel supports group communication, allowing guests to either create rooms other guests can occupy or enter rooms other guests are currently in. When a guest creates a room, Hotel responds with a unique UUID key that can be shared with others so they can also join the room. If the guest creates a room with a display name, then that room is considered public and will be included in a listing other guests request for viewing available rooms. Once in a room, if a guest decides to send a specific update message, Hotel will forward it to any other guests in the same room. If all guests leave a room, then the room is deleted and no one can try to join it with the UUID that was used to represent it.

The other default service that exists is called Suitcase. Hotel does not buffer any messages sent. A guest entering a room does not receive any messages previously sent. Suitcase allows for key-value pairs to be set in the server. These values can be retrieved at any time. These key-value pairs will exist until cleared by setting the value to be nonexistent.

Prosign, in the ecosystem of Golang, is just considered a library. Anyone can install and import it into their code to be used. This import allows someone to configure and extend the server how they like. Developers who wanted to create their own service would only have to implement the three functions of its interface. The OnGuestArival function is called by the server when a new guest connects. The OnGuestDeparture function is called by the server when a connection with the guest has ended. The Forward function is called when the server has determined the message from a guest is meant for that specific service. Services do not have to worry about networking so developing and testing them is trivial.

### Hosting yourself

Honestly you'll probably just want to contact me. Here it is anyway:

1. Get access to a server.
2. Install [docker](https://www.docker.com/)
3. Clone https://gitlab.com/prosign/prosign
4. Edit docker-compose.yml to change the port you want it running on. For example if you want things to run on port 6666 you would change the line to: 3000:6666
5. Run docker-compose up -d

Once you've done all of this you're server should be running. To check on the status you can look into docker logs. You can check if the container is actually running by using docker container ls.

## UI Gen

Procedurally create Canvas objects from code. This is used for configuring properties of sensors that then get set in anvel. This process uses ananoymous functions (delegates) heavily so it is worth your time to learn them if you haven't already. Getting this project working was pretty annoying, requiring my to prefab a lot of stuff. If you have to edit these prefabs, you'll have to re-calculate the bundles it is using. You'll have to do that by right-clicking in the scene and selecting to "rebuild all bundles" or something like that.

### Example

#### Code

```c#
var onChange = delegate (float x) { Debug.Log(x); };
var formatValue = delegate (float x) { return x.ToString("0.00"); };
var onButtonPressed = delegate () { Debug.Log("button pressed"); };

var windowElements = new IElement[] {
    new SliderElement(0, 1, .5f, onChange, formatValue),
    new ButtonElement("Save", onButtonPressed),
};
var window = new Window("Set Value", windowElements);
var view = new View(window);
view.Build(Vector3.zero, Vector3.zero, new Vector2(1, 1.2f));
```

#### Generates

![Cool](https://i.imgur.com/8e9yRWN.png)

## This Project

### Showcase

The showcase is pretty much the main scene of the entire project. It takes a long time to load cause their's a lot of stuff in it.

#### Subsribing To Car Changes

There's a lot of different components in the scene that care about the cars loaded. To keep up with all of this I have chosen to use a publisher subscriber pattern, similar to [Observer pattern](https://sourcemaking.com/design_patterns/observer). A CarManager uses both Singletons and events to accomplish this goal. If you are unfamiliar with either of those go back to step one of this document (Topics to understand for the project).

Basically, if you always want to stay updated on what is the main car that is currently loaded, then provide the CarManager  a anonymous function to call, which looks like this:

```C#
CarManager
    .Instance()
    .OnMainCarChange += delegate(PictureItem newCar) 
    {
        // Do stuff with the new car.
    };
```

You can set the main car that is currently loaded through the CarManager class as well as subscribe to changes in the Garage. The garage just means all available cars that we can look at. Subscribing to the garage is sometimes useful for when the scene first starts up (in the case of the table of cars).

#### Changing Controls

![Control UML](https://i.imgur.com/OlCj5Au.png)

The two important things to know if you want to create your own control is how PlayerControl and PlayerControlBehavior works. PlayerControl is an abstract class that has the function Build which takes a hand and returns an action. What you're custom control class should do is inherit from PlayerControl and implement this command. The action you return after building is responsible for un-doing everything your build command has done. When the user changes controls away from your's PlayerControlBehavior is going to call that action Build returned and it's going to expect everything to be removed. If you're unsucesful in cleaning things up that will easily result in bugs. Especially when it comes time to rebuild your control.

An simple example of this process is the GrabPlayerControl:

```C#
public class GrabPlayerControl : PlayerControl
{

    public override Action Build(VRTK_ControllerEvents hand)
    {
        var touch = hand.gameObject.AddComponent<VRTK_InteractTouch>();
        var grab = GrabControlBehavior.Initialize(hand);

        return delegate ()
        {
            UnityEngine.Object.Destroy(grab);
            UnityEngine.Object.Destroy(touch);
        };
    }

    public override Texture2D GetIcon()
    {
        return Resources.Load<Texture2D>("PlayerControl/Grab-icon");
    }
    
}
```

Everything it's created in the Build is being removed in the delegate provided. If you wanted to add variables to configure the control, you would do it in the child class itself. For example, if I wanted to limit grab distance or something, I could do something that might look like:

```C#
public class GrabPlayerControl : PlayerControl
{

    private float maxGrabDistance;

    public GrabPlayerControl(float maxGrabDistance)
    {
        this.maxGrabDistance = maxGrabDistance;
    }

    public override Action Build(VRTK_ControllerEvents hand)
    {
        var touch = hand.gameObject.AddComponent<VRTK_InteractTouch>();
        
        var grab = GrabControlBehavior.Initialize(hand);
        grab.SetMaxDistance(maxGrabDistance);

        return delegate ()
        {
            UnityEngine.Object.Destroy(grab);
            UnityEngine.Object.Destroy(touch);
        };
    }

    public override Texture2D GetIcon()
    {
        return Resources.Load<Texture2D>("PlayerControl/Grab-icon");
    }
    
}
```

This is pretty convenient cause now you can very quickly change parameters just by changing what you pass the grab control. Also you can choose different parameters for different hands. Food for thought.

To use the controls you have built you will need to build a controller config, then pass is the hand you want to configure. Example code could look like:

```C#
var config = new ControllerConfig(new List<PlayerControl>()
{
    new GrabPlayerControl(),
    new TeleportPlayerControl(),
    new SelectPlayerControl(),
    new MyNewCustomControl()
});

config.Build(leftHand);
```

Calling build will set up the VRTK Wheel radial, build all the icons, and select the first control passed in (GrabPlayerControl in this case). 

### "Source Control"

When I talk about source control in this section, I am not speaking of git, but actually a custom program for making changes in lidar positioning persistant accross runs of the unity scene. Right now it only works for Lidar but can be easily extended to other sensors like the camera.

A repository refers to a collection of configured cars. An artifact refers to a specific car's configuration. Each artifact is stored as it's own seperate file. Each repository is a folder of artifact files. Artifacts are stored as json to it's easy to open up and make changes or verify things are writing to disk correctly. Artifacts are a collection of sensor configurations.

The showcase has it's own SensorManager that takes these sensor configurations and maps them to the current sensors in the scene. Loading a new car causes artifacts to be loaded which then have their configurations applied to the sensors.

To save changes to you're repository (changes to sensor configuations) you must call Commit(). Without this your changes will be lost. Currently commit is called whenever the car loaded changes.

### Controlling The Display Of The Loaded Car

If you want to change the lexus car model to something else, modify what CarFactory.MakeBigCar() returns.