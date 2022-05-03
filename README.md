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
این پروژه از نوع asp.net core  web Api است ارسال اطلاعات از دستور زیر استفاده نمایید :


</p>
 <b>
  curl -X 'GET' \
  'https://localhost:7157/LastTrade/{id}?date=2020-02-02' \
  -H 'accept: application/json'
  </b>
