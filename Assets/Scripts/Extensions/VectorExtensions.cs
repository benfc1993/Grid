using UnityEngine;

namespace Extensions
{
    public enum VectorRotation
    {
        Quarter, 
        Half, 
        ThreeQuarters
    }
    
    public static class VectorExtensions
    {
        public static Vector2 Rotate(this Vector2 vector, VectorRotation rotation)
        {
            Vector2 rotatedVector = vector;
            switch (rotation)
            {
                case VectorRotation.Quarter:
                    rotatedVector = new Vector2(vector.y, -vector.x);
                    break;
                case VectorRotation.Half:
                    rotatedVector = new Vector2(-vector.x, -vector.y);
                    break;
                case VectorRotation.ThreeQuarters:
                    rotatedVector = new Vector2(-vector.y, vector.x);
                    break;
            }

            return rotatedVector;
        }
        
        public static Vector2 Rotate(this Vector2 vector, VectorRotation rotation, Vector2 origin)
        {
            Vector2 rotatedVector = vector - origin;
            switch (rotation)
            {
                case VectorRotation.Quarter:
                    rotatedVector = new Vector2(rotatedVector.y, -rotatedVector.x);
                    break;
                case VectorRotation.Half:
                    rotatedVector = new Vector2(-rotatedVector.x, -rotatedVector.y);
                    break;
                case VectorRotation.ThreeQuarters:
                    rotatedVector = new Vector2(-rotatedVector.y, rotatedVector.x);
                    break;
            }

            return rotatedVector + origin;
        }
        
        public static Vector2 Rotate(this Vector2 vector, VectorRotation rotation, Vector3 origin)
        {
            Vector2 originVector2 = new Vector2(origin.x, origin.z);
            Vector2 rotatedVector = vector - originVector2;
            
            switch (rotation)
            {
                case VectorRotation.Quarter:
                    rotatedVector = new Vector2(rotatedVector.y, -rotatedVector.x);
                    break;
                case VectorRotation.Half:
                    rotatedVector = new Vector2(-rotatedVector.x, -rotatedVector.y);
                    break;
                case VectorRotation.ThreeQuarters:
                    rotatedVector = new Vector2(-rotatedVector.y, rotatedVector.x);
                    break;
            }

            return rotatedVector + originVector2;
        }
        
        public static Vector2Int Rotate(this Vector2Int vector, VectorRotation rotation)
        {
            Vector2Int returnVector = vector;
            switch (rotation)
            {
                case VectorRotation.Quarter:
                    returnVector = new Vector2Int(vector.y, -vector.x);
                    break;
                case VectorRotation.Half:
                    returnVector = new Vector2Int(-vector.x, -vector.y);
                    break;
                case VectorRotation.ThreeQuarters:
                    returnVector = new Vector2Int(-vector.y, vector.x);
                    break;
            }

            return returnVector;
        }
        
        public static Vector2 RotateCCW(this Vector2 vector, VectorRotation rotation)
        {
            Vector2 rotatedVector = vector;
            switch (rotation)
            {
                case VectorRotation.Quarter:
                    rotatedVector = new Vector2(-vector.y, vector.x);
                    break;
                case VectorRotation.Half:
                    rotatedVector = new Vector2(-vector.x, -vector.y);
                    break;
                case VectorRotation.ThreeQuarters:
                    rotatedVector = new Vector2(vector.y, -vector.x);
                    break;
            }

            return rotatedVector;
        }
        
        public static Vector2 RotateCCW(this Vector2 vector, VectorRotation rotation, Vector2 origin)
        {
            Vector2 rotatedVector = vector - origin;
            switch (rotation)
            {
                case VectorRotation.Quarter:
                    rotatedVector = new Vector2(-rotatedVector.y, rotatedVector.x);
                    break;
                case VectorRotation.Half:
                    rotatedVector = new Vector2(-rotatedVector.x, -rotatedVector.y);
                    break;
                case VectorRotation.ThreeQuarters:
                    rotatedVector = new Vector2(rotatedVector.y, -rotatedVector.x);
                    break;
            }

            return rotatedVector + origin;
        }
        
        public static Vector2 RotateCCW(this Vector2 vector, VectorRotation rotation, Vector3 origin)
        {
            Vector2 originVector2 = new Vector2(origin.x, origin.z);
            Vector2 rotatedVector = vector - originVector2;
            
            switch (rotation)
            {
                case VectorRotation.Quarter:
                    rotatedVector = new Vector2(-rotatedVector.y, rotatedVector.x);
                    break;
                case VectorRotation.Half:
                    rotatedVector = new Vector2(-rotatedVector.x, -rotatedVector.y);
                    break;
                case VectorRotation.ThreeQuarters:
                    rotatedVector = new Vector2(rotatedVector.y, -rotatedVector.x);
                    break;
            }
            
            return rotatedVector + originVector2;
        }
        
        public static Vector2Int RotateCCW(this Vector2Int vector, VectorRotation rotation)
        {
            Vector2Int returnVector = vector;
            switch (rotation)
            {
                case VectorRotation.Quarter:
                    returnVector = new Vector2Int(-vector.y, vector.x);
                    break;
                case VectorRotation.Half:
                    returnVector = new Vector2Int(-vector.x, -vector.y);
                    break;
                case VectorRotation.ThreeQuarters:
                    returnVector = new Vector2Int(vector.y, -vector.x);
                    break;
            }

            return returnVector;
        }
    }
}