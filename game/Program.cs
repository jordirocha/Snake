#region SNAKE: Sets global variables

Console.OutputEncoding = System.Text.Encoding.UTF8;

// The velocity of snake on moving
var ms = 200;
// The position on snake starts
var x = 70;
var y = 11;
// The snake, a list of coordinates
var snake = new List<(int, int)> { new(item1: x, item2: y) };
// The position of food
var foodX = 0;
var foodY = 0;
GenerateFood();
var validKey = new ConsoleKeyInfo();

#endregion

#region SNAKE: Draws countour

for (var i = 0; i < Console.WindowWidth; i++)
{
    Console.SetCursorPosition(left: i, top: 0);
    Console.Write(value: "*");
    Console.SetCursorPosition(left: i, top: Console.WindowHeight - 1);
    Console.Write(value: "*");
}

for (var i = 0; i < Console.WindowHeight; i++)
{
    Console.SetCursorPosition(left: 0, top: i);
    Console.Write(value: "*");
    Console.SetCursorPosition(left: Console.WindowWidth - 1, top: i);
    Console.Write(value: "*");
}

#endregion

#region SNAKE: Game On

Start();

#endregion

#region SNAKE: Game development

/// <summary>
///    Run the game
/// </summary>
void Start()
{
    // To detect what key will be pressed
    var keyInfo = Console.ReadKey();
    while (true)
    {
        ms = snake.Count switch
        {
            < 5 => 150,
            < 10 => 100,
            < 20 => 90,
            _ => 60
        };
        while (Console.KeyAvailable) keyInfo = Console.ReadKey(intercept: true);
        CheckAteFood();
        Direction(keyPress: keyInfo);
        CheckCollision();
    }
}

/// <summary>
///    Checks if the head has ate food, if ate generates food
/// </summary>
void CheckAteFood()
{
    if (foodX == snake[index: snake.Count - 1].Item1 && snake[index: snake.Count - 1].Item2 == foodY)
    {
        // Increase the size of snake
        snake.Add(item: new ValueTuple<int, int>(item1: snake[index: snake.Count - 1].Item1,
            item2: snake[index: snake.Count - 1].Item2));
        GenerateFood();
    }
}

/// <summary>
///    Generates new food in the map, cheking previously that the food is not in the coordenates of snake or countours
/// </summary>
void GenerateFood()
{
    var positionX = 0;
    var positionY = 0;
    do
    {
        var random = new Random();
        positionX = random.Next(minValue: 1, maxValue: Console.WindowWidth - 2);
        positionY = random.Next(minValue: 1, maxValue: Console.WindowHeight - 2);
    } while (!IsAvailable(x: positionX, y: positionY));

    foodX = positionX;
    foodY = positionY;
    Console.SetCursorPosition(left: foodX, top: foodY);
    Console.Write(value: "Ó");
}

/// <summary>
///    Cheks that food is not in the coordenates of the snake
/// </summary>
bool IsAvailable(int x, int y)
{
    for (var i = 0; i < snake.Count; i++)
        if (snake[index: i].Item1 == x && snake[index: i].Item2 == y)
            return false;
    return true;
}

/// <summary>
///    Cheks what key was preseed and change the direction of snake and printing it
/// </summary>
void Direction(ConsoleKeyInfo keyPress)
{
    validKey = keyPress.Key is ConsoleKey.UpArrow or ConsoleKey.DownArrow or ConsoleKey.LeftArrow
        or ConsoleKey.RightArrow or ConsoleKey.W or ConsoleKey.E or ConsoleKey.S or ConsoleKey.D
        ? keyPress
        : keyPress = validKey;
    for (var i = snake.Count - 1; i >= 0; i--)
    {
        Console.SetCursorPosition(left: snake[index: i].Item1, top: snake[index: i].Item2);
        Console.Write(value: "■");
    }
    
    Console.SetCursorPosition(left: snake[index: snake.Count - 1].Item1, top: snake[index: snake.Count - 1].Item2);
    Console.Write(value: keyPress.Key is ConsoleKey.UpArrow ? "▼" :
        keyPress.Key is ConsoleKey.LeftArrow or ConsoleKey.W ? "►" :
        keyPress.Key is ConsoleKey.RightArrow or ConsoleKey.E ? "◄" : "▲");
    Thread.Sleep(millisecondsTimeout: ms);
    Console.SetCursorPosition(left: snake[index: 0].Item1, top: snake[index: 0].Item2);
    Console.Write(value: " ");
    snake.RemoveAt(index: 0);
    if (keyPress.Key == ConsoleKey.UpArrow && keyPress.Key != ConsoleKey.DownArrow)
        snake.Add(item: new ValueTuple<int, int>(item1: x, item2: --y));
    if (keyPress.Key == ConsoleKey.LeftArrow) snake.Add(item: new ValueTuple<int, int>(item1: --x, item2: y));
    if (keyPress.Key == ConsoleKey.RightArrow) snake.Add(item: new ValueTuple<int, int>(item1: ++x, item2: y));
    if (keyPress.Key == ConsoleKey.DownArrow) snake.Add(item: new ValueTuple<int, int>(item1: x, item2: ++y));
    if (keyPress.Key == ConsoleKey.W) snake.Add(item: new ValueTuple<int, int>(item1: --x, item2: --y));
    if (keyPress.Key == ConsoleKey.E) snake.Add(item: new ValueTuple<int, int>(item1: ++x, item2: --y));
    if (keyPress.Key == ConsoleKey.S) snake.Add(item: new ValueTuple<int, int>(item1: --x, item2: ++y));
    if (keyPress.Key == ConsoleKey.D) snake.Add(item: new ValueTuple<int, int>(item1: ++x, item2: ++y));
}

/// <summary>
///    Cheks if the snake get out of courtains of eats itself, in that case the game ends
/// </summary>
void CheckCollision()
{
    if (snake[index: snake.Count - 1].Item1 <= 0 || snake[index: snake.Count - 1].Item1 >= Console.WindowWidth - 1)
        Environment.Exit(exitCode: 0);
    if (snake[index: snake.Count - 1].Item2 <= 0 || snake[index: snake.Count - 1].Item2 >= Console.WindowHeight - 1)
        Environment.Exit(exitCode: 0);
    for (var i = 0; i < snake.Count - 1 && snake.Count > 1; i++)
        if (snake[index: i].Item1 == snake[index: snake.Count - 1].Item1 &&
            snake[index: snake.Count - 1].Item2 == snake[index: i].Item2)
            Environment.Exit(exitCode: 0);
}

#endregion