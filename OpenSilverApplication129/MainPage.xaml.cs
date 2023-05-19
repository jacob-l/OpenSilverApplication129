using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OpenSilverApplication129
{
    public class EventPublisher
    {
        // Declare the event using EventHandler<T> 
        public event EventHandler<string> OnSomethingHappened;

        public void DoSomething()
        {
            // When something happens, invoke the event
            OnSomethingHappened?.Invoke(this, "Something just happened!");
        }
    }

    public class EventSubscriber
    {
        public EventSubscriber(EventPublisher publisher)
        {
            // Start the Stopwatch just before subscription
            var stopwatch = Stopwatch.StartNew();

            for (var i = 0; i < 100; i++)
            {
                var j = i;
                publisher.OnSomethingHappened += (sender, e) => { Console.WriteLine($"Handled event: {e} " + j); };
            }

            // Stop the Stopwatch immediately after subscription
            stopwatch.Stop();
            Console.WriteLine($"Event subscription time: {stopwatch.Elapsed.TotalMilliseconds} ms");
        }
    }

    public partial class MainPage : Page
    {
        readonly EventPublisher _publisher = new EventPublisher();

        public MainPage()
        {
            this.InitializeComponent();

            // Enter construction logic here...
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            OpenSilver.Interop.ExecuteJavaScriptVoid("console.clear()");
            Console.WriteLine("LOADED");
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < 100; i++)
            {
                var j = i;
                Button.Click += (s, a) => { Console.WriteLine("Button Clicked " + j); };
            }
            sw.Stop();
            Console.WriteLine($"Event subscription time: {sw.Elapsed.TotalMilliseconds} ms");

            new EventSubscriber(_publisher);

            //publisher.DoSomething();  // This will trigger the event
        }
    }
}
