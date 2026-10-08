// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Crypto
{
    /// <summary>
    /// A certain elliptic curve point
    /// </summary>
    #if EXPORT_HAMPER_CORE_CRYPTO
	public
	#else
	internal
	#endif
    interface ICurvePoint<T>
		where T : struct, ICurvePoint<T>
	{
		/// <summary>
		/// Gets the amount of data segments this integer is based on
		/// </summary>
		int SegmentCount
		{
			get;
		}
		
		/// <summary>
		/// X coord of this curve point
		/// </summary>
		public Span<UInt64> X
		{
			get;
		}
		/// <summary>
		/// X coord of this curve point
		/// </summary>
		public Span<UInt64> Y
		{
			get;
		}

		/// <summary>
		/// Gets if the curve point has valid data assigned
		/// </summary>
		public bool IsValid
		{
			get;
		}
		
		/// <summary>
		/// Instantiates a new object of <typeparamref name="T"/>
		/// </summary>
		/// <returns>The new object instance</returns>
		static abstract T Create(int size);
    }
}