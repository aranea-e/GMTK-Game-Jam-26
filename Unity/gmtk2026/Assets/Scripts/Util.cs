using UnityEngine;
public static class Util {

    public static Vector2 mousePosition() {
        Vector3 point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 
                                    8);
        return Camera.main.ScreenToWorldPoint(point);
    }
    public static Vector2 mousePositionInt() {
        return new Vector2(Mathf.Floor(mousePosition().x), Mathf.Floor(mousePosition().y));
    }

    public static bool mouseInBounds(Vector2 low, Vector2 high) {
        return low.x <= mousePosition().x &&
               low.y <= mousePosition().y &&
               high.x >= mousePosition().x &&
               high.y >= mousePosition().y;
    }
}
