﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data
{
    public class MathHelper
    {
        /// <summary>
        /// A global <see cref="System.Random"/> instance.
        /// </summary>
        public static Random Random = new Random();

        /// <summary>
        /// Maps a float from 0...360 to 0...255
        /// </summary>
        /// <param name="value"></param>
        public static byte CreateRotationByte(float value)
        {
            return (byte)(((value % 360) / 360) * 256);
        }

        public static int CreateAbsoluteInt(double value)
        {
            return (int)(value * 32);
        }

        /// <summary>
        /// Gets a byte representing block direction based on the rotation
        /// of the entity that placed it, on a flat plane.
        /// </summary>
        public static Direction DirectionByRotationFlat(Entity p, bool invert = false)
        {
            byte direction = (byte)((int)Math.Floor((p.Yaw * 4F) / 360F + 0.5D) & 3);
            if (invert)
                switch (direction) // TODO: Don't cast these
                {
                    case 0: return (Direction)2;
                    case 1: return (Direction)5;
                    case 2: return (Direction)3;
                    case 3: return (Direction)4;
                }
            else
                switch (direction)
                {
                    case 0: return (Direction)3;
                    case 1: return (Direction)4;
                    case 2: return (Direction)2;
                    case 3: return (Direction)5;
                }
            return 0;
        }

        /// <summary>
        /// Gets a byte representing block direction based on the rotation
        /// of the entity that placed it.
        /// </summary>
        public static Direction DirectionByRotation(PlayerEntity p, Vector3 position, bool invert = false)
        {
            // TODO: Figure out some algorithm based on player's look yaw
            double d = Math.Asin((p.Position.Y - position.Y) / position.DistanceTo(p.Position));
            if (d > (Math.PI / 4)) return invert ? (Direction)1 : (Direction)0;
            if (d < -(Math.PI / 4)) return invert ? (Direction)0 : (Direction)1;
            return DirectionByRotationFlat(p, invert);
        }

        /// <summary>
        /// Gets a byte representing block direction based on the rotation
        /// of the entity that placed it.
        /// </summary>
        public static Vector3 FowardVector(Entity entity, bool invert = false)
        {
            Direction value = (Direction)DirectionByRotationFlat(entity, invert);
            switch (value)
            {
                case Direction.East:
                    return Vector3.East;
                case Direction.West:
                    return Vector3.West;
                case Direction.North:
                    return Vector3.North;
                case Direction.South:
                    return Vector3.South;
                default:
                    return Vector3.Zero;
            }
        }

        public static double Distance2D(double a1, double a2, double b1, double b2)
        {
            return Math.Sqrt(Math.Pow(b1 - a1, 2) + Math.Pow(b2 - a2, 2));
        }

        public static Vector3 RotateX(Vector3 vector, double rotation) // TODO: Matrix
        {
            rotation = -rotation; // the algorithms I found were left-handed
            return new Vector3(
                vector.X,
                vector.Y * Math.Cos(rotation) - vector.Z * Math.Sin(rotation),
                vector.Y * Math.Sin(rotation) + vector.Z * Math.Cos(rotation));
        }

        public static Vector3 RotateY(Vector3 vector, double rotation)
        {
            rotation = -rotation; // the algorithms I found were left-handed
            return new Vector3(
                vector.Z * Math.Sin(rotation) + vector.X * Math.Cos(rotation),
                vector.Y,
                vector.Z * Math.Cos(rotation) - vector.X * Math.Sin(rotation));
        }

        public static Vector3 RotateZ(Vector3 vector, double rotation)
        {
            rotation = -rotation; // the algorithms I found were left-handed
            return new Vector3(
                vector.X * Math.Cos(rotation) - vector.Y * Math.Sin(rotation),
                vector.X * Math.Sin(rotation) + vector.Y * Math.Cos(rotation),
                vector.Z);
        }

        public static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        /// <summary>
        /// Returns a value indicating the most extreme value of the
        /// provided Vector.
        /// </summary>
        public static unsafe CollisionPoint GetCollisionPoint(Vector3 velocity)
        {
            // TODO: Does this really need to be so unsafe
            int index = 0;
            void* vPtr = &velocity;
            double* ptr = (double*)vPtr;
            double max = 0;
            for (int i = 0; i < 3; i++)
            {
                double value = *(ptr + i);
                if (max < Math.Abs(value))
                {
                    index = i;
                    max = Math.Abs(value);
                }
            }
            switch (index)
            {
                case 0:
                    if (velocity.X < 0)
                        return CollisionPoint.NegativeX;
                    return CollisionPoint.PositiveX;
                case 1:
                    if (velocity.Y < 0)
                        return CollisionPoint.NegativeY;
                    return CollisionPoint.PositiveY;
                default:
                    if (velocity.Z < 0)
                        return CollisionPoint.NegativeZ;
                    return CollisionPoint.PositiveZ;
            }
        }
    }

    public enum Direction
    {
        Bottom = 0,
        Top = 1,
        North = 2,
        South = 3,
        West = 4,
        East = 5
    }

    public enum CollisionPoint
    {
        PositiveX,
        NegativeX,
        PositiveY,
        NegativeY,
        PositiveZ,
        NegativeZ
    }
}
