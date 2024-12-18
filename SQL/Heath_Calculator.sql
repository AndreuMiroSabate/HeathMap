SELECT round(Z+(select ABS(min(round(Z))+1) from player_positions)) *1000 + round(X+(select ABS(min(round(X))+1) from player_positions)) as HeathID,
round(X) AS X, round(Z) AS Z, count(*) as Heath 
FROM andreums.player_positions
group by 1