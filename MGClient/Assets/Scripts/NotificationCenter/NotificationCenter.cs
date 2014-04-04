using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//    NotificationCenter is used for handling messages between GameObjects.
//    GameObjects can register to receive specific notifications.  When another objects sends a notification of that type, all GameObjects that registered for it and implement the appropriate message will receive that notification.
//    Observing GameObjetcs must register to receive notifications with the AddObserver function, and pass their selves, and the name of the notification.  Observing GameObjects can also unregister themselves with the RemoveObserver function.  GameObjects must request to receive and remove notification types on a type by type basis.
//    Posting notifications is done by creating a Notification object and passing it to PostNotification.  All receiving GameObjects will accept that Notification object.  The Notification object contains the sender, the notification type name, and an option hashtable containing data.
//    To use NotificationCenter, either create and manage a unique instance of it somewhere, or use the static NotificationCenter.
 

public class NotificationCenter{
	//instance model
	private static NotificationCenter instance;
	public static NotificationCenter Instance()
	{
		if(instance == null)
		{
			instance = new NotificationCenter();
		}
		return instance;
	}
	
	NotificationCenter()
	{
		instance = this;
	}
	static Hashtable notifications = new Hashtable();
	
	public static void AddObserver (Component observer, string name)
	{
		if(name == null || name == "")
		{
			Debug.Log("Null name specified for notification in AddObserver.");
			return;
		}
		if(notifications.Contains(name) == false)
		{
			notifications[name] = new ArrayList();
		}
		ArrayList notifyList = (ArrayList)notifications[name];
		if(notifyList.Contains(observer) == false)
		{
			notifyList.Add(observer);
		}
	}
	
	public static void RemoveObserver (Component observer, string name)
	{
		ArrayList notifyList = (ArrayList)notifications[name];
		if(notifyList.Contains(observer))
			notifyList.Remove(observer);
		if(notifyList.Count == 0)
			notifications.Remove(name);
	}
	
	public static void PostNotification(Component aSender, string aName)
	{
		PostNotification<Object>(new Notification<Object>(aSender, aName));
	}
	
	public static void PostNotification<Object>(Component aSender, string aName, Object aData)
	{
		PostNotification<Object>(new Notification<Object>(aSender, aName, aData));
	}
	
	public static void PostNotification<T>(Notification<T> aNotification)
	{
		if (aNotification.name == null || aNotification.name == "") 
		{
			Debug.Log("Null name sent to PostNotification.");
			return; 
		}
		
		ArrayList notifyList = (ArrayList)notifications[aNotification.name];
		if (notifyList == null) {
			Debug.Log("Notify list not found in PostNotification."); 
			return; 
		}
		ArrayList observersToRemove = new ArrayList();
		foreach(Component observer in notifyList)
		{
			if(observer == null)
			{
				observersToRemove.Add(observer);
			}
			else
			{
				observer.SendMessage(aNotification.name, aNotification.data, SendMessageOptions.DontRequireReceiver);
			}
		}
		
		foreach(Component observer in observersToRemove)
		{
			notifyList.Remove(observer);
		}
	}
	
	public class Notification<T>
	{
		public Component sender;
		public string name;
		public T data;
		public Notification(Component aSender, string aName)
		{
			sender = aSender;
			name = aName;
		}
		public Notification(Component aSender, string aName, T aData)
		{
			sender = aSender;
			name = aName;
			data = aData;
		}
	}
	
} 
