SELECT round(Z) *1000 + round(X) as HeathID,round(Z), round(X), count(*) as Heath 
FROM andreums.player_positions
group by 1