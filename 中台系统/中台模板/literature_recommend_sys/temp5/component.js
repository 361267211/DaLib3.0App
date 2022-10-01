var literature_recommend_sys_temp5_html = `<div class="featured-book-list-pad">
  <div class="featured-book-list">
    <div class="top-main-title-temp"><span>特色书单</span><span class="e-title">/ Featured Book List</span><span class="r-btn">查看更多 ></span></div>
    <div class="featured-book-warp clearFloat">
        <div class="col1"></div>
        <div class="col2"></div>
        <div class="col3">
            <div class="col4"></div>
            <div class="col5"></div>
        </div>
    </div>
  </div>
</div><!--特色书单-->`;
function literature_recommend_sys_temp5(){
	var list = document.getElementsByClassName('literature_recommend_sys_temp5');
	for(var i = 0;i<list.length;i++){
		if(list[i].getAttribute('class').indexOf('jl_vip_zt_vray')<0){
			list[i].setAttribute('class','literature_recommend_sys_temp5 jl_vip_zt_vray jl_vip_zt_warp');
      var literature_recommend_sys_temp5_set_list = null;
      if(list[i].dataset && list[i].dataset.set){
        literature_recommend_sys_temp5_set_list = JSON.parse(list[i].dataset.set.replace(/'/g,'"'));
      }
			new Vue({
        el: '#'+list[i].lastChild.id,
        template: literature_recommend_sys_temp5_html,
        data: {
          literature_recommend_sys_temp5_menu_num:0,//当前菜单
          literature_recommend_sys_temp5_menu_list:[],//菜单列表加数据
          literature_recommend_sys_temp5_list:[],//当前列表数据
          post_url_vip:'http://192.168.21.46:8000',
        },
        mounted(){
          if(literature_recommend_sys_temp5_set_list){
            if(literature_recommend_sys_temp5_set_list.length>0){
              var list=[];
              literature_recommend_sys_temp5_set_list.forEach((it,i) => {
                var topCount = '';
                var columnid = '';
                var OrderRule = '';
                if(it){
                  topCount = it['topCount']||16;
                  columnid = it['id'];
                  OrderRule = it['sortType'];
                }
                list.push({Count:topCount,ColumnId:columnid,OrderRule:OrderRule});
              });
              this.literature_recommend_sys_temp5_initData(list);
            }
          }
        },
        methods:{
          menuClick(val){
            this.literature_recommend_sys_temp5_menu_num = val;
            this.literature_recommend_sys_temp5_menu_list = this.literature_recommend_sys_temp5_list[val]['titleInfos']||[];
          },
          literature_recommend_sys_temp5_openurl(url){
            window.location.href = url;
          },
          literature_recommend_sys_temp5_initData(list){
            var post_url = this.post_url_vip+'/articlerecommend/api/sceneuse/getsceneusebyidbatch';
            axios({
              url: post_url,
              method: 'post',
              data:list,
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if(res.data && res.data.statusCode==200){
                this.literature_recommend_sys_temp5_list = res.data.data||[];
                if(res.data.data.length>0){
                  this.literature_recommend_sys_temp5_menu_list = res.data.data[0].titleInfos||[];
                }

              }
            }).catch(err => {
              console.log(err);
            });
          },
        },
      });
		}
	}
  }
  literature_recommend_sys_temp5()