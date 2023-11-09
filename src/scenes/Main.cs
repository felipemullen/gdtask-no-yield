using Fractural.Tasks;
using Godot;

public partial class Main : Node
{
    public override void _Ready()
    {
        GD.Print("Running a task from a non-async method.");
        Run().Forget();
    }

    public async GDTaskVoid Run()
    {
        GD.Print("delay 100 frames");
        await GDTask.DelayFrame(100);

        GD.Print("waiting some amount of time");
        await GDTask.Delay(TimeSpan.FromSeconds(10));

        GD.Print("Waiting a single frame");
        await GDTask.Yield();
        await GDTask.NextFrame();
        await GDTask.WaitForEndOfFrame();

        GD.Print("Waiting for specific lifetime call");
        await GDTask.WaitForPhysicsProcess();

        GD.Print("Cancellation");
        var cts = new CancellationTokenSource();
        ReallyLongTask(cts.Token).Forget();
        await GDTask.Delay(TimeSpan.FromSeconds(3));
        cts.Cancel();

        GD.Print("Async await with return value");
        string result = await RunWithResult();

        GD.Print("Received result" + result);
    }

    public async GDTask<string> RunWithResult()
    {
        await GDTask.Delay(TimeSpan.FromSeconds(3));
        return "A result string";
    }

    public async GDTaskVoid ReallyLongTask(CancellationToken cancellationToken)
    {
        GD.Print("Starting long task.");
        await GDTask.Delay(TimeSpan.FromSeconds(1000000), cancellationToken: cancellationToken);
        GD.Print("Finished long task.");
    }
}
