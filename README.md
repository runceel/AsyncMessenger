AsyncMessenger
==============

MVVM Light Messenger extension.
This library provide async/await friendly extension methods.

- SendAsync method.
- ReceiveAsyncMessage method.


Support platform
----------------

- .NET Framework 4.5
- Windows store app(8.0, 8.1)
- Windows Phone for Silverlight(8.9, 8.1)
- Windows Phone app(8.1)
- Xamarin.Android

Code snippet. 
---------------

### Send

```cs
bool result = await Messenger.Default
    .SendAsync<NotificationMessage, bool>(new NotificationMessage("Are you OK?");
// do something...
```

### Receive

```cs
// RegisterAsyncMessage return value must have reference. If not reference then Callback will unregist from Messenger.
IDisposable token = Messenger.Default
	.RegisterAsyncMessage<NotificationMessage, bool>(m =>
    {
		var dialogResult = MessageBox.Show(m.Notification, "", MessageBoxButton.OKCancel);
		return Task.FromResult(dialogResult == MessageBoxResult.OK);
	}));
```

How to use
--------------

- Create project.
- Install AsyncMvvmMessenger from NuGet.
	- Related package
		- Portable.MvvmLightLib
- Create ViewModel


```cs
public class SampleViewModel : ViewModelBase
{
	private RelayCommand _alertCommand;
	public RelayCommand AlertCommand
	{
		get
		{
			return _alertCommand ?? (_alertCommand = new RelayCommand(this.AlertExecute));
		}
	}
	
	private async void AlertExecute()
	{
		// Write logic here!
	}
}
```


- Write ViewModel logic

```cs
// create message
var message = new NotificationMessage<string>("content", "notification");

// send async message(generic parameter is Message type and Return type.)
var result = await Messenger.Default
    .SendAsync<NotificationMessage<string>, bool>(message);
if (result)
{
    // do something
}
else
{
    // do something
}
```

- Create receiver

```cs
// write receive code. global area. eq: App.OnStartup event handler.

// Generic parameter is Message type and Return type.
Messenger.Default
    .ReceiveAsyncMessage<NotificationMessage<string>, bool>(receiveMessage =>
        // receive logic.
		var returnValue = MessageBox.Show(
			receiveMessage.Content, 
			receiveMessage.Notification, MessageBoxButton.OKCancel) == MessageBoxResult.OK;
        return Task.FromResult<bool>(returnValue);
    }));
```

If universal Windows app...

```cs
// write receive code. global area. eq: App.OnLaunched method.

// Generic parameter is Message type and Return type.
Messenger.Default
    .ReceiveAsyncMessage<NotificationMessage<string>, bool>(async receiveMessage =>
        var dialog = new MessageDialog(receiveMessage.Content, receiveMessage.Notification);
        var 
    }));
```
