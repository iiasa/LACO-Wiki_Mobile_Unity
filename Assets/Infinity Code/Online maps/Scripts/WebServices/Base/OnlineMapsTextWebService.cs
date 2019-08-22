/*     INFINITY CODE 2013-2018      */
/*   http://www.infinity-code.com   */

using System;

/// <summary>
/// The base class for working with the web services returns text response.
/// </summary>
public abstract class OnlineMapsTextWebService: OnlineMapsWebServiceAPI
{
    /// <summary>
    /// Event that occurs when a response is received from webservice.
    /// </summary>
    public Action<string> OnComplete;

    public new Action<OnlineMapsTextWebService> OnFailed;
    public new Action<OnlineMapsTextWebService> OnFinish;
    public new Action<OnlineMapsTextWebService> OnSuccess;

    protected string _response;

    /// <summary>
    /// Gets a response from webservice.
    /// </summary>
    /// <value>
    /// The response.
    /// </value>
    public string response
    {
        get { return _response; }
    }

    public override void Destroy()
    {
        if (OnDispose != null) OnDispose(this);

        www = null;
        _response = string.Empty;
        _status = OnlineMapsQueryStatus.disposed;
        customData = null;
        OnComplete = null;
        OnFinish = null;
    }

    /// <summary>
    /// Checks whether the response from webservice.
    /// </summary>
    protected void OnRequestComplete(OnlineMapsWWW www)
    {
        if (www == null || !www.isDone) return;

        _status = www.hasError ? OnlineMapsQueryStatus.error : OnlineMapsQueryStatus.success;
        _response = _status == OnlineMapsQueryStatus.success ? www.text : www.error;

        if (OnComplete != null) OnComplete(_response);
        if (status == OnlineMapsQueryStatus.success)
        {
            if (OnSuccess != null) OnSuccess(this);
        }
        else
        {
            if (OnFailed != null) OnFailed(this);
        }
        if (OnFinish != null) OnFinish(this);

        _status = OnlineMapsQueryStatus.disposed;
        _response = null;
        this.www = null;
        customData = null;
    }
}