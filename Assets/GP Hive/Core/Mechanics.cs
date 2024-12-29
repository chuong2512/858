using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mechanics
{

    public static T GetObjectWithTouchRay<T>()
    {
        T t = default;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            t = hit.collider.GetComponent<T>();
        }

        return t;
    }
    public static T GetObjectWithTouchRay<T>(string tag)
    {
        T t = default;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.CompareTag(tag))
            {
                t = hit.collider.GetComponent<T>();
            }
        }

        return t;
    }

    public static T GetObjectWithTouchRay<T>(string tag, LayerMask layer)
    {
        T t = default;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layer))
        {
            if (hit.collider.CompareTag(tag))
            {
                t = hit.collider.GetComponent<T>();
            }
        }

        return t;
    }

    public static T GetObjectWithTargetedRay<T>(Transform target)
    {
        T t = default;

        Ray ray = new Ray(Camera.main.transform.position, target.position -
        Camera.main.transform.position);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            t = hit.collider.GetComponent<T>();
        }

        return t;
    }

    public static T GetObjectWithTargetedRay<T>(string tag, Transform target)
    {
        T t = default;

        Ray ray = new Ray(Camera.main.transform.position, target.position -
        Camera.main.transform.position);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.CompareTag(tag))
            {
                t = hit.collider.GetComponent<T>();
            }
        }

        return t;
    }

    public static T GetObjectWithTargetedRay<T>(string tag, Transform target, LayerMask layer)
    {
        T t = default;

        Ray ray = new Ray(Camera.main.transform.position, target.position -
        Camera.main.transform.position);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layer))
        {
            if (hit.collider.CompareTag(tag))
            {
                t = hit.collider.GetComponent<T>();
            }
        }

        return t;
    }

    public static List<T> GetObjectsWithTouchRay<T>()
    {
        List<T> ls = new List<T>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] allHits = Physics.RaycastAll(ray, 100f);
        if (allHits.Length > 0)
        {
            foreach (RaycastHit h in allHits)
            {
                T t = h.collider.GetComponent<T>();

                if (t != null)
                {
                    ls.Add(t);
                }
            }
        }

        return ls;
    }

    public static List<T> GetObjectsWithTouchRay<T>(string tag)
    {
        List<T> ls = new List<T>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] allHits = Physics.RaycastAll(ray, 100f);
        if (allHits.Length > 0)
        {
            foreach (RaycastHit h in allHits)
            {
                if (h.collider.CompareTag(tag))
                {
                    T t = h.collider.GetComponent<T>();

                    if (t != null)
                    {
                        ls.Add(t);
                    }
                }
            }
        }

        return ls;
    }

    public static List<T> GetObjectsWithTouchRay<T>(string tag, LayerMask layer)
    {
        List<T> ls = new List<T>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] allHits = Physics.RaycastAll(ray, 100f, layer);
        if (allHits.Length > 0)
        {
            foreach (RaycastHit h in allHits)
            {
                if (h.collider.CompareTag(tag))
                {
                    T t = h.collider.GetComponent<T>();

                    if (t != null)
                    {
                        ls.Add(t);
                    }
                }
            }
        }

        return ls;
    }
}
