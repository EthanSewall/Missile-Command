using static Raylib_cs.Raylib;  
using static Raylib_cs.Color;   
using static Raylib_cs.Raymath; 
using System.Numerics; 
using System;
using System.Collections.Generic;

namespace MissileCommand
{
    public class Window
    {
        const int screenWidth = 1600;
        const int screenHeight = 900;

        public static int Width()
        {
            return screenWidth;
        }
        public static int Height()
        {
            return screenHeight;
        }

        public static int Main()
        {
            InitWindow(screenWidth, screenHeight, "Project");
            HideCursor();
            SetTargetFPS(60);
            bool stillGoing = true;
            Raylib_cs.Color background = new Raylib_cs.Color(200, 40, 0, 255);

            PlayerCursor player = new PlayerCursor();
            Launcher launcher1 = new Launcher(10, new Vector2(130, screenHeight - 150));
            Launcher launcher2 = new Launcher(20, new Vector2(screenWidth / 2 - 10, screenHeight - 175));
            Launcher launcher3 = new Launcher(10, new Vector2(screenWidth - 150, screenHeight - 150));
            Location loc1 = new Location(new Vector2(275, screenHeight - 125));
            Location loc2 = new Location(new Vector2(425, screenHeight - 125));
            Location loc3 = new Location(new Vector2(575, screenHeight - 125));
            Location loc4 = new Location(new Vector2(975, screenHeight - 125));
            Location loc5 = new Location(new Vector2(1125, screenHeight - 125));
            Location loc6 = new Location(new Vector2(1275, screenHeight - 125));
            List<Projectile> projectiles = new List<Projectile>();
            List<Incoming> inbound = new List<Incoming>();
            int currentLevel = 1;
            float incomingTime = 0;
            int remainingIncoming = 20;
            float incomingSpeed = 4;
            int score = 0;
            float levelTime = 0;
            float incomingDelay = 3f;
            bool inGame = false;

            while (!WindowShouldClose() && stillGoing)  // Game loop continues until ESC pressed, window closed, or the boolean becomes false
            {
                BeginDrawing();
                ClearBackground(BLACK);
                DrawBackground(currentLevel, background, score);

                if (inGame)
                {
                    {
                        loc1.Draw();
                        loc2.Draw();
                        loc3.Draw();
                        loc4.Draw();
                        loc5.Draw();
                        loc6.Draw();
                    }// draws the locations

                    {
                        launcher1.DisplayText();
                        launcher2.DisplayText();
                        launcher3.DisplayText();
                    }//displays remaining projectiles
                    for (int i = 0; i < projectiles.Count; i++)
                    {
                        if (projectiles[i].active)
                        {
                            projectiles[i].Update();
                            projectiles[i].Draw();
                        }
                        else
                        {
                            projectiles.RemoveAt(i);
                        }
                    }//updates projectiles that are still active, removes ones that aren't
                    {
                        if (IsKeyPressed(Raylib_cs.KeyboardKey.KEY_ONE) && launcher1.remaining > 0)
                        {
                            projectiles.Add(new Projectile(launcher1.speed, launcher1.pos, GetMousePosition()));
                            launcher1.remaining--;
                        }
                        if (IsKeyPressed(Raylib_cs.KeyboardKey.KEY_TWO) && launcher2.remaining > 0)
                        {
                            projectiles.Add(new Projectile(launcher2.speed, launcher2.pos, GetMousePosition()));
                            launcher2.remaining--;
                        }
                        if (IsKeyPressed(Raylib_cs.KeyboardKey.KEY_THREE) && launcher3.remaining > 0)
                        {
                            projectiles.Add(new Projectile(launcher3.speed, launcher3.pos, GetMousePosition()));
                            launcher3.remaining--;
                        }
                    }// spawns projectiles on key press

                    for (int i = 0; i < inbound.Count; i++)
                    {
                        inbound[i].Update();
                        inbound[i].Draw();
                        bool cont = false;
                        foreach (Projectile pro in projectiles)
                        {
                            if (pro.exploding && Vector2.Distance(inbound[i].pos, pro.pos) < 50)
                            {
                                inbound.RemoveAt(i);
                                score += 10;
                                cont = true;
                            }
                        }
                        if (cont)
                        {
                            continue;
                        }

                        if (inbound[i].pos.Y > screenHeight - 125)
                        {
                            switch (inbound[i].target.X)
                            {
                                case 300:
                                    loc1.Destroy();
                                    inbound.RemoveAt(i);
                                    break;
                                case 450:
                                    loc2.Destroy();
                                    inbound.RemoveAt(i);
                                    break;
                                case 600:
                                    loc3.Destroy();
                                    inbound.RemoveAt(i);
                                    break;
                                case 1000:
                                    loc4.Destroy();
                                    inbound.RemoveAt(i);
                                    break;
                                case 1150:
                                    loc5.Destroy();
                                    inbound.RemoveAt(i);
                                    break;
                                case 1300:
                                    loc6.Destroy();
                                    inbound.RemoveAt(i);
                                    break;
                            }
                        }
                    }//updates Incomings, checks if they've been intercepted
                    incomingTime += GetFrameTime();
                    if (incomingTime > incomingDelay)
                    {
                        Random random = new Random();
                        int incomingTarget = random.Next(1, 7);

                        bool validTarget = false;

                        if (!(loc1.destroyed && loc2.destroyed && loc3.destroyed && loc4.destroyed && loc5.destroyed && loc6.destroyed) && remainingIncoming > 0)
                        {
                            while (!validTarget)
                            {
                                switch (incomingTarget)
                                {
                                    case 1:
                                        if (!loc1.destroyed)
                                        {
                                            inbound.Add(new Incoming(incomingSpeed, loc1.pos + new Vector2(25, 0)));
                                            validTarget = true;
                                        }
                                        break;
                                    case 2:
                                        if (!loc2.destroyed)
                                        {
                                            inbound.Add(new Incoming(incomingSpeed, loc2.pos + new Vector2(25, 0)));
                                            validTarget = true;
                                        }
                                        break;
                                    case 3:
                                        if (!loc3.destroyed)
                                        {
                                            inbound.Add(new Incoming(incomingSpeed, loc3.pos + new Vector2(25, 0)));
                                            validTarget = true;
                                        }
                                        break;
                                    case 4:
                                        if (!loc4.destroyed)
                                        {
                                            inbound.Add(new Incoming(incomingSpeed, loc4.pos + new Vector2(25, 0)));
                                            validTarget = true;
                                        }
                                        break;
                                    case 5:
                                        if (!loc5.destroyed)
                                        {
                                            inbound.Add(new Incoming(4, loc5.pos + new Vector2(25, 0)));
                                            validTarget = true;
                                        }
                                        break;
                                    case 6:
                                        if (!loc6.destroyed)
                                        {
                                            inbound.Add(new Incoming(4, loc6.pos + new Vector2(25, 0)));
                                            validTarget = true;
                                        }
                                        break;
                                }//decides where each Incoming is pointed
                                incomingTarget = random.Next(1, 7);
                            }
                            incomingTime = 0;
                            remainingIncoming--;
                        }
                        else if (!(loc1.destroyed && loc2.destroyed && loc3.destroyed && loc4.destroyed && loc5.destroyed && loc6.destroyed) && remainingIncoming == 0)
                        {
                            levelTime += GetFrameTime();
                            if (levelTime > 3)
                            {
                                currentLevel++;

                                score += 5 * launcher1.remaining;
                                score += 5 * launcher3.remaining;
                                score += 10 * launcher2.remaining;
                                if (!loc1.destroyed)
                                {
                                    score += 15;
                                }
                                if (!loc2.destroyed)
                                {
                                    score += 15;
                                }
                                if (!loc3.destroyed)
                                {
                                    score += 15;
                                }
                                if (!loc4.destroyed)
                                {
                                    score += 15;
                                }
                                if (!loc5.destroyed)
                                {
                                    score += 15;
                                }
                                if (!loc6.destroyed)
                                {
                                    score += 15;
                                }


                                launcher1.remaining = 10;
                                launcher2.remaining = 10;
                                launcher3.remaining = 10;
                                loc1.destroyed = false;
                                loc2.destroyed = false;
                                loc3.destroyed = false;
                                loc4.destroyed = false;
                                loc5.destroyed = false;
                                loc6.destroyed = false;
                                incomingSpeed *= 1.05f;
                                if (incomingSpeed > 20f)
                                {
                                    incomingSpeed = 20f;
                                }
                                incomingDelay *= 0.95f;
                                if (incomingDelay < 0.25f)
                                {
                                    incomingDelay = 0.25f;
                                }
                                background = new Raylib_cs.Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255), 255);
                                incomingTime = 0;
                                remainingIncoming = 20;
                                levelTime = 0;

                                for (int i = 5; i < 51; i += 5)
                                {
                                    if (currentLevel > i)
                                    {
                                        remainingIncoming++;
                                    }
                                }

                                if (currentLevel > 50)
                                {
                                    int additionalIncoming = currentLevel - 50;

                                    launcher1.remaining += additionalIncoming;
                                    launcher3.remaining += additionalIncoming;
                                    remainingIncoming += remainingIncoming * 2;
                                }
                            }

                        }//advancing levels
                    }

                    if (loc1.destroyed && loc2.destroyed && loc3.destroyed && loc4.destroyed && loc5.destroyed && loc6.destroyed)
                    {
                        DrawText("GAME END", (screenWidth / 2) - 500, (screenHeight / 2) - 100, 200, new Raylib_cs.Color(100, 0, 0, 255));
                    }
                }
                else
                {
                    if(IsMouseButtonPressed(Raylib_cs.MouseButton.MOUSE_LEFT_BUTTON))
                    {
                        inGame = true;
                    }
                }


                player.Update();
                player.Draw();

                EndDrawing();
            }
            CloseWindow();

            return 0;
        }

        static void DrawBackground(int level, Raylib_cs.Color bgColor, int score)
        {
            DrawRectangle(0, screenHeight - 100, screenWidth, 100, bgColor);
            DrawRectangle(40, screenHeight - 150, 200, 50, bgColor);
            DrawRectangle(screenWidth - 240, screenHeight - 150, 200, 50, bgColor);
            DrawRectangle((screenWidth / 2) - 150, screenHeight - 175, 300, 75, bgColor);
            DrawText("Level: " + level.ToString(), 0, 0, 40, YELLOW);
            DrawText(score.ToString(), screenWidth / 2, 0, 40, YELLOW);
        }
    }

    class PlayerCursor
    {
        public Vector2 pos = new Vector2();
        Vector2 size = new Vector2(15, 5);

        public void Update()
        {
            Vector2 mPos = GetMousePosition();
            pos = mPos;
        }

        public void Draw()
        {
            DrawRectangleV(pos, size, PURPLE);
            DrawRectangleV(pos - new Vector2(size.X - size.Y, 0), size, PURPLE);
            DrawRectangleV(pos - new Vector2(0, size.X - size.Y), new Vector2(size.Y, size.X), PURPLE);
            DrawRectangleV(pos, new Vector2(size.Y, size.X), PURPLE);
        }
    }

    class Launcher
    {
        public Vector2 pos;
        public float speed;
        public int remaining;

        public Launcher(float newSpeed, Vector2 newPos)
        {
            pos = newPos;
            speed = newSpeed;
            remaining = 10;
        }

        public void DisplayText()
        {
            DrawText(remaining.ToString(), (int)pos.X - 10, (int)pos.Y, 50, YELLOW);
        }

        //used only for checking position
        public Vector2 size = new Vector2(20, 10);
        Raylib_cs.Color color = new Raylib_cs.Color(40, 200, 0, 255);
        public void Draw()
        {
            DrawRectangleV(pos, size, color);
        }
    }

    class Projectile
    {
        public Vector2 pos;
        float speed;
        public Vector2 target;
        public Vector2 direction;
        public bool active;
        public bool exploding;
        float explosionDelay;

        public Projectile(float newSpeed, Vector2 newPos, Vector2 newTarget)
        {
            speed = newSpeed;
            pos = newPos;
            target = newTarget;

            if (target.Y > pos.Y)
            {
                target.Y = pos.Y;
            }

            Vector2 v = pos - target;
            direction = new Vector2(v.X / v.Length(), v.Y / v.Length());
            active = true;
        }

        public void Update()
        {
            if (!exploding)
            {
                if (Vector2.Distance(pos, target) > speed / 2)
                {
                    pos -= new Vector2(direction.X * speed, direction.Y * speed);
                }
                else
                {
                    exploding = true;
                }
            }
            else
            {
                explosionDelay += GetFrameTime();

                if (explosionDelay > 0.25f)
                {
                    active = false;
                }
            }


        }

        public void Draw()
        {
            if (!exploding)
            {
                DrawCircleV(pos, 5, RAYWHITE);
            }
            else
            {
                DrawCircleV(pos, 50, ORANGE);
            }
        }
    }

    class Location
    {
        public bool destroyed;
        public Vector2 pos;

        public Location(Vector2 newPos)
        {
            pos = newPos;
            destroyed = false;
        }

        public void Draw()
        {
            if (!destroyed)
            {
                DrawRectangleV(pos, new Vector2(50, 25), BLUE);
            }
        }

        public void Destroy()
        {
            destroyed = true;
        }
    }

    class Incoming
    {
        public Vector2 pos;
        public Vector2 target;
        Vector2 direction;
        float speed;

        public Incoming(float newSpeed, Vector2 newTarget)
        {
            speed = newSpeed;
            target = newTarget;
            Random random = new Random();
            pos = new Vector2(random.Next(0, 1600), 0);

            Vector2 v = pos - target;
            direction = new Vector2(v.X / v.Length(), v.Y / v.Length());
        }

        public void Update()
        {
            pos -= new Vector2(direction.X * speed, direction.Y * speed);
        }

        public void Draw()
        {
            DrawCircleV(pos, 5, RAYWHITE);
        }
    }
}