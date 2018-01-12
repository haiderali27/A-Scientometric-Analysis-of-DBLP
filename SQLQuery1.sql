
select top (10)  * from(
select min(authored) as authored, sum(publication_count) as publication_count,min(article_journal) as article_journal
from coauthorship_index
group by authored,article_journal
) as A
order by publication_count desc;





