//We are using...
//HAL.cs
//App.cs

partial class App {
    public static partial void start() {

    }
    public static partial void tick() {
        //HAL.drawText("TestString", 50, 50, 16);
        HAL.drawRect(20, 20, 5, 5, 255, 0, 20, 255);
    }
    public static partial void end() {

    }
}