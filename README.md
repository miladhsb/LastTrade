<h2>کوئری دریافت اطلاعات نماد</h2>


                 SELECT Trade.*, Instrument.*
                FROM Instrument INNER JOIN
                Trade ON Instrument.Id = Trade.InstrumentId
                
                ,(SELECT InstrumentId, MAX(DateTimeEn) AS DateTimeEn
                FROM  Trade
                WHERE DateTimeEn>=@TradeDate
                GROUP BY InstrumentId ) LastTrade

                WHERE Trade.InstrumentId=LastTrade.InstrumentId 
                AND Trade.DateTimeEn=LastTrade.DateTimeEn


<p>
این پروژه از نوع asp.net core  web Api است پس از اجرای پروژه جهت ارسال درخواست  به صورت زیر عمل کنید :


</p>
 <b>
https://localhost:7157/LastTrade?date=2020-03-03
  </b>
