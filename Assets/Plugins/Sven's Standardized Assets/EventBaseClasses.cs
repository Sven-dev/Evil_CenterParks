using System;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// A Container for all types of unity events.
/// </summary>

#region Generic
[Serializable]
public class UnityVoidEvent : UnityEvent { }

[Serializable]
public class UnityIntEvent : UnityEvent<int> { }

[Serializable]
public class UnityStringEvent : UnityEvent<string> { }
[Serializable]
public class UnityFloatEvent : UnityEvent<float> { }

[Serializable]
public class UnityBoolEvent : UnityEvent<bool> { }
#endregion

#region Unity
[Serializable]
public class UnitySpriteEvent : UnityEvent<Sprite> { }
#endregion

#region Custom
[Serializable]
public class UnityTimeSpanEvent : UnityEvent<TimeSpan> { }
#endregion