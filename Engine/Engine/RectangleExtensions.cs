#region File Description
//-----------------------------------------------------------------------------
// RectangleExtensions.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using Engine;

namespace Platformer
{
    /// <summary>
    /// A set of helpful methods for working with rectangles.
    /// </summary>
    public static class RectangleExtensions
    {
        /// <summary>
        /// Calculates the signed depth of intersection between two rectangles.
        /// </summary>
        /// <returns>
        /// The amount of overlap between two intersecting rectangles. These
        /// depth values can be negative depending on which wides the rectangles
        /// intersect. This allows callers to determine the correct direction
        /// to push objects in order to resolve collisions.
        /// If the rectangles are not intersecting, Vector2.Zero is returned.
        /// </returns>
        public static Vector GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            double halfWidthA = rectA.Width / 2.0f;
            double halfHeightA = rectA.Height / 2.0f;
            double halfWidthB = rectB.Width / 2.0f;
            double halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            Vector centerA = new Vector(rectA.Left + halfWidthA, rectA.Top + halfHeightA, 0);
            Vector centerB = new Vector(rectB.Left + halfWidthB, rectB.Top + halfHeightB, 0);

            // Calculate current and minimum-non-intersecting distances between centers.
            double distanceX = centerA.X - centerB.X;
            double distanceY = centerA.Y - centerB.Y;
            double minDistanceX = halfWidthA + halfWidthB;
            double minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector.Zero;

            // Calculate and return intersection depths.
            double depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            double depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector(depthX, depthY, 0);
        }

        /// <summary>
        /// Gets the position of the center of the bottom edge of the rectangle.
        /// </summary>
        public static Vector GetBottomCenter(this Rectangle rect)
        {
            return new Vector(rect.X + rect.Width / 2.0f, rect.Bottom, 0);
        }
    }
}
