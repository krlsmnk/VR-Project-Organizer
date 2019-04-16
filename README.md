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
9. Design Patterns
    * [Singletons](https://twitter.com/jasperstocker/status/1105478975588106242/photo/1)
    * Factory
    * Momentos
10. Anonymous functions (delegates) like [Func](https://docs.microsoft.com/en-us/dotnet/api/system.func-1?view=netframework-4.7.2) and [Action](https://docs.microsoft.com/en-us/dotnet/api/system.action-1?view=netframework-4.7.2).
11. C#'s [event](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/event) keyword. (VRTK uses it heavily, I use it in some places) 
12. C# Naming/Styling conventions.
13. C# [Namespaces](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/namespaces/) 
14. LEARN THE REFACTOR TOOLS OF VISUAL STUDIOS. LET THE EDITOR WRITE THE CODE FOR YOU
15. `string.Format()`
16. Avoid booleans for variables. Look into [enum](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/enum)s

Personal conventions on project:

1. Any code goes in the Script folder.
2. Create a folder per scene.
3. Namespaces follow folder structure once inside Scripts folder, and are prepended with `CAVS.ProjectOrganizer`.
    * For example, if a script is located under Scripts/Interaction, the namespace would then be `CAVS.ProjectOrganizer.Interaction`.
4. Any class that inherits from MonoBehavior has their name appended with "Behavior".
    * For example, If I see a class named `GameManagerBehavior`, I know it inherits from monobehavior and if I have an instance of it, it must exist in the scene.
    * If the class is just named `GameManager`, then I know it does not inherit from monobehavior, and it's existance is not tied to the scene loaded.
5. Each Scene will have their own `GameManagerBehavior` class that is attatched to an object in the scene named "__SCRIPTS\__".
6. Avoid the use of tags as much as possible.
7. 

RULES TO HOLD YOURSELF TO:
1. Don't change existing libraries code unless absilutely necessary. IE: Don't change code under the VRTK folder. It's almost always possible to extend functionality in some way through options like inheritance. In the entire lifetime of the project I have had to make changes once.
2. Be very very very very very very very thoughtful with your variable, function, and class naming. This proper naming servese as the best form of documentation.

## Some Tips

### Constructors for Monobehavior

Unity prevents classes who inherit from monobehaior from having their own constructors. This makes sense, as those types of scripts must be attatched to game objects. However it is annoying not having a nice function call that will automatically create one, while allowing you to set it's variables simultaniously. To get around this I have created a static method on certain MonoBehavior classes in which I desire that functionality called `Initialize()`. Some of these functions take extra parameters specific to the class. For example, this is what part of GrabBehaviorLooks like:

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

### Not Bothering With Comments

So what is this project? I think it's best explained in this graph:

![Graph](https://i.imgur.com/nUdDtOp.png)

Our Goal is not clear, nor is our solution. So we have ourselves an [extreme project](https://en.wikipedia.org/wiki/Extreme_project_management) which means I've been having to do [extreme programming](https://en.wikipedia.org/wiki/Extreme_programming) (a poor job of it at that). If we had frequent communication with the client this project would have resembled more of an extreme one, but it's honestly thoe whole thing has felt how [southpark determines property value](https://youtu.be/xGTIKTH28BY?t=368).

Anyways what does this have to do with comments? With extreme programming and this project comes accepting that whatever you do can/will be discarded. Almost the entirity of the code under `Scripts/Project` (a lot) is no longer being used. Nor is any of the networking code I wrote is being used. Spending time commenting every bit of that code would have taken forever, and ultimately a waste of time.

More than just being a waste of time, there are very good arguments as to why you don't want to overly comment your code. It can be summarised as:

> Good Code is Self Documenting

Focus writing small functions with good names. Look at this not so pretty code: 

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

There's a going on to load this image. More importantly it's doing multiple jobs to accomplish it's goal, the function can be broken up.

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
    string hashName = CalculateMD5Hash(url);
    string[] chunks = url.Split('.');
    string extension = chunks[chunks.Length - 1];
    return Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "cache"), hashName + "." + extension);
}

/// <summary>
/// Calculates the MD5 Hash of a string
/// </summary>
/// <param name="input">the string to calculate an MD5 hash from</param>
/// <returns>Md% hash</returns>
private static string CalculateMD5Hash(string input)
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

These functions work as black boxes. You don't really have to care about how the MD5 hash is being calculated, you just need to know that the function does that. And guess how you know the function does that? By it's name. Making any comments like I've done on it is fucking **redundant** and wastes time. By the way, *this is image loading code that is no longer being used.*

And probably the biggest reason not to leave comments in an extreme project: **things change**. I can change a function name by hitting f2 and it'll be renamed throughout the entire project, keeping everything up to date. You know what won't change? The comment I left explaining it and what it does. The compiler doesn't care about my comments. It won't tell me when my comment is out of date, that I'm telling another developer a lie. The comment will just lay their unchanged cause that function edit was one of 20 I am going to be making that single commit. And That commit is going to be 1 of 100s in the project. You know what will happen though? Another developer is going to come along and instead of reading the code itself, they will take the comment at face value and make assumptions that are no longer true. What comes next is a nice long debug session for something that could have been avoided if better naming of methods where employed.

There are still scenarios in which I comment my code. Comments are sometimes necessary unfortunatly. Their especially important if you're building libraries for hundres/thousands of other developers to use. Libraries that have clear goals. Just, take away from this section:

> Refactor your comments into better code

/rant

# Project Organization

This project has been broken up into smaller projects for doing very specific things. Much of the intracies of ANVEL have been encapsulated into it's own unity project. Prosign (The server code) is spread across 3 repositories. The UI Gen also has it's own repository. 

Here are their repositories respectfully:
1. [Prosign](https://gitlab.com/prosign)
2. [UI Gen](https://github.com/EliCDavis/UI-Gen)
3. [Anvel](https://github.com/EliCDavis/Anvel-To-Unity)
4. [This project](https://github.com/EliCDavis/VR-Project-Organizer)

## Anvel

## Prosign

The entirety of the server side code for Prosign was written in the programming language Golang and deployed using the containerization software called Docker. A test-driven approach was adopted early on to ensure new code coming into the project does not break existing functionality. Golang's [mockgen tool](https://github.com/golang/mock) was used to automatically generate mock structs that implement interfaces defined in the code to write more sophisticated unit tests. Some of these more involved tests attempt to ensure the server responds appropriately to failure at different points in execution. Perhaps the most useful tests in the development of the server were the integration tests. In these tests, both the server and two clients were spawned and one client would attempt to talk to the other by sending and receiving messages from the server. To write these tests, the clients would have to implement the protocol to properly communicate with the server. Once these tests successfully passed not only had the server been tested from end to end, but now there exists an example in Golang of how to communicate with the server for reference. Gitlab's Continuous Integration server was used to automatically build and run all the tests that existed with the code to ensure nothing was broken, as well as compute test coverage to ensure only code that has been tested is being committed. At the time of this writing, 81.5\% of the server's code is covered by tests. 

![Architecture](https://i.imgur.com/HBmBMWV.png)

An instance of a running Prosign server is made up of a collection of services and guests. A high-level architectural outline can be seen in \figureref{Architecture}. All services used by the server are registered at startup. Throughout the lifetime of the server, the services will receive messages from multiple guests. A guest is something that can both send and receive messages, as well as be notified of errors. A guest was originally just a TCP socket but has since been abstracted away into its own interface. In doing so services can communicate with many different types of guests with no extra work. If the server was ever required to be able to communicate with a web page through web-sockets, the only code that would need to be added is strictly web-socket code that implements the guest interface. Once completed, two-way communication between a web page and a TCP socketed application like Unity3D would be possible. Implementing the guest interface to account for executables that were launched by the application would be trivial due to the fact Golang's executables and sockets share the same read and write interfaces.

### Protocol

Incoming messages from a guest are arranged into three parts outlined in \figureref{messagestructure}. The first part is the message size which is represented as a single byte. The second part is the header which contains two components: the service for the message and the functionality requested from the service known as the method. The third part of the message is the message body which is the remaining payload for any additional information. The message body is optional depending on the service and method in question. A service is not required to send a response back to the client.

![message structure](https://i.imgur.com/KeeHXgO.png)

Messages sent from the server are constructed almost exactly the same way as the client. The one difference is that one extra byte is added at the very beginning of the message to denote status. A status of 0 means the message is a normal message, and a status of 1 means the message is an error message stating something illegal was attempted. An error message is always a response to a message the client had sent previously and can be formulated by either the service or the server itself. The body of the message is the description of the error.

### Services

The first default service in Prosign is called Hotel. Hotel supports group communication, allowing guests to either create rooms other guests can occupy or enter rooms other guests are currently in. When a guest creates a room, Hotel responds with a unique UUID key that can be shared with others so they can also join the room. If the guest creates a room with a display name, then that room is considered public and will be included in a listing other guests request for viewing available rooms. Once in a room, if a guest decides to send a specific update message, Hotel will forward it to any other guests in the same room. If all guests leave a room, then the room is deleted and no one can try to join it with the UUID that was used to represent it.

The other default service that exists is called Suitcase. Hotel does not buffer any messages sent. A guest entering a room does not receive any messages previously sent. Suitcase allows for key-value pairs to be set in the server. These values can be retrieved at any time. These key-value pairs will exist until cleared by setting the value to be nonexistent.

Prosign, in the ecosystem of Golang, is just considered a library. Anyone can install and import it into their code to be used. This import allows someone to configure and extend the server how they like. Developers who wanted to create their own service would only have to implement the three functions of its interface. The \verb+OnGuestArival+ function is called by the server when a new guest connects. The \verb+OnGuestDeparture+ function is called by the server when a connection with the guest has ended. The \verb+Forward+ function is called when the server has determined the message from a guest is meant for that specific service. Services do not have to worry about networking so developing and testing them is trivial.

## UI Gen

## This Project