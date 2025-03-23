<template>
    <div class="box_error">
        <div class="error">
            <div class="code">
                <p>404</p>
            </div>
            <div class="msg_en">
                <p>Page</p>
                <p>Not</p>
                <p>Found</p>
            </div>
        </div>
        <div class="msg_ja">お探しのページは見つかりませんでした。</div>
    </div>
</template>

<style scoped>
.box_error {
    width: 460px;
    height: 260px;
    border: 1px solid #aaa;
}

.error {
    display: flex;
    gap: 20px;
    align-items: center;
    justify-content: center;
}

.code {
    font-size: 64px;
    font-weight: bold;
    color: #888;
}

.msg_en p {
    margin: 0;
    font-size: 16px;
}

.msg_ja {
    font-size: 18px;
    text-align: center;
}

</style>