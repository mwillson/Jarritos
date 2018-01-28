using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionObject
{
    public float x;
    public float y;
    public float w;
    public float h;
    public int motion;
}

public class ComputedMotion
{
    public float vx;
    public float vy;
    public int motion;
}

public class CustomCollision : MonoBehaviour {

    public MotionObject ball;
    public MotionObject paddle;
    public MotionObject playfield;
    public MotionObject target;
    public float InputMultiplier = 1.0f;

    // intersecting aabb (aabb 1 and aabb 0 overlap)
    bool aabbintersect(float x0, float y0, float w0, float h0, float x1, float y1, float w1, float h1)
    {
        return !(y1 + h1 < y0 || x1 + w1 < x0 || x0 + w0 < x1 || y0 + h0 < y1);
    }
    // inside aabb (aabb 1 is inside aabb 0)
    bool aabbinside(float x0, float y0, float w0, float h0, float x1, float y1, float w1, float h1)
    {
        return (y1 + h1 < y0 || x1 + w1 < x0 + w0 || x0 + w0 > x1 + w1 || y0 + h0 < y1 + h1);
    }

    float clampw(float x0, float w0, float x1, float w1)
    {
        if (x0 < x1)
            x0 = x1;
        if (x0 + w0 > x1 + w1)
            x0 = (x1 + w1) - w0;
        return x0;
    }

    float center(float x, float w)
    {
        return x + w / 2;
    }

	private ComputedMotion cm = new ComputedMotion ();

    private ComputedMotion getMotion(int motion)
    {
        cm.vx = 0;
        cm.vy = 0;
        cm.motion = motion;

        switch (motion)
        {
            case 0:
                cm.vx = 0;
                cm.vy = 0;
                break;
            case 1:
                cm.vx = 1;
                cm.vy = 0;
                break;
            case 2:
                cm.vx = 1;
                cm.vy = 1;
                break;
            case 3:
                cm.vx = 0;
                cm.vy = -1;
                break;
            case 4:
                cm.vx = -1;
                cm.vy = -1;
                break;
            case 5:
                cm.vx = -1;
                cm.vy = 0;
                break;
            case 6:
                cm.vx = -1;
                cm.vy = 1;
                break;
            case 7:
                cm.vx = 0;
                cm.vy = 1;
                break;
            case 8:
                cm.vx = 1;
                cm.vy = 1;
                break;
        }
        return cm;
    }

    private int bounce(int motion)
    {
        int result = motion;
        switch(motion)
        {
            case 0:
                result = 0;
                break;
            case 1:
                result = 2;
                break;
            case 2:
                result = 3;
                break;
            case 3:
                result = 4;
                break;
            case 4:
                result = 5;
                break;
            case 5:
                result = 6;
                break;
            case 6:
                result = 7;
                break;
            case 7:
                result = 8;
                break;
            case 8:
                result = 1;
                break;
        }
        return result;
    }

    private void RunStep()
    {

        bool ballbounced = false;

        // 1. Paddle clamp and ball-against-paddle bounce

        paddle.x = clampw(paddle.x, paddle.w, playfield.x, playfield.w);
        
        if (aabbintersect(ball.x,ball.y,ball.w,ball.h,paddle.x,paddle.y,paddle.w,paddle.h))
        {
            float ballc = center(ball.x, ball.w);
            float paddlec = center(paddle.x, paddle.w);
            // TODO use ballc and paddlec to force the bounce in some direction
            ball.motion = bounce(ball.motion);
        }

        ComputedMotion ballm = getMotion(ball.motion);

        // 2. x axis motion

        float prevx = ball.x;
        float prevy = ball.y;

        ball.x += ballm.vx;

        if (!aabbinside(playfield.x, playfield.y, playfield.w, playfield.h, ball.x, ball.y, ball.w, ball.h))
        {
            ball.x = clampw(ball.x, ball.w, playfield.x, playfield.w);
            ballbounced = true;
        }
        if (aabbintersect(ball.x, ball.y, ball.w, ball.h, target.x, target.y, target.w, target.h))
        {
            if (prevx < ball.x)
                ball.x = target.x - ball.w;
            else // (prevx > ball.x)
                ball.x = target.x + target.w;
        }

        // 2. y axis motion

        ball.y += ballm.vy;

        if (!aabbinside(playfield.x, playfield.y, playfield.w, playfield.h, ball.x, ball.y, ball.w, ball.h))
        {
            ball.y = clampw(ball.y, ball.h, playfield.y, playfield.h);
            ballbounced = true;
        }
        if (aabbintersect(ball.x, ball.y, ball.w, ball.h, target.x, target.y, target.w, target.h))
        {
            if (prevy < ball.y)
                ball.y = target.y - ball.h;
            else // (prevy > ball.y)
                ball.y = target.y + target.h;
        }

        if (ballbounced)
        {
            ball.motion = bounce(ball.motion);
        }

        // TODO: target motion (apply similarly to ball motion)

    }

}
